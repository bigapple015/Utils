using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Cares.Service.Dtos;
using Cares.Service.Models;
using NaomsSyncInterface.Model;

namespace NaomsSyncInterface.Utils
{
    /// <summary>
    /// MQ消费者
    /// </summary>
    public static class MQConsumer
    {
        /// <summary>
        /// MQ地址
        /// </summary>
        private const string MQUri = "NaomsMQUri";
        /// <summary>
        /// 创建连接工厂
        /// </summary>
        private static IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 连接
        /// </summary>
        private static IConnection connection = null;

        /// <summary>
        /// 会话
        /// </summary>
        private static ISession session = null;

        /// <summary>
        /// 是否活动
        /// </summary>
        private static volatile bool isAlive = false;

        /// <summary>
        /// 消费者
        /// </summary>
        private static IMessageConsumer consumer = null;
        /// <summary>
        /// 初始化消费者
        /// </summary>
        /// <returns></returns>
        public static bool InitConsumer()
        {
            try
            {
                if (connection != null)
                {
                    CloseConsumer();
                }

                LogHelper.Info("开始初始化MQConsumer");
                //通过工厂构建连接
                connection = factory.CreateConnection();
                connection.ExceptionListener += connection_ExceptionListener;
                connection.ConnectionInterruptedListener += connection_ConnectionInterruptedListener;
            
                //创建回话
                session = connection.CreateSession();
                //创建消费者
                consumer = session.CreateConsumer(session.GetDestination(ConfigUtils.GetString("NaomsConsumerTopic"), DestinationType.Topic));
                //注册监听事件
                consumer.Listener += consumer_Listener;
                if (!connection.IsStarted)
                {
                    //启动连接
                    connection.Start();
                }
                LogHelper.Info("MQ初始化成功");
                isAlive = true;
                return true;
            }
            catch (Exception ex)
            {
                isAlive = false;
                LogHelper.Error("MQ初始化失败："+ex);
                return false;
            }
        }

        /// <summary>
        /// 消费者监听
        /// </summary>
        /// <param name="message"></param>
        private static void consumer_Listener(IMessage message)
        {
            isAlive = true;
            try
            {
                #region 获取消息
                ITextMessage txtMsg = message as ITextMessage;
                if (txtMsg == null || string.IsNullOrWhiteSpace(txtMsg.Text))
                {
                    LogHelper.Info("MQ接收到消息为空");
                    return;
                }
                else
                {
                    LogHelper.Info("接收到MQ消息为："+txtMsg.Text);
                }
                #endregion

                ParseXml(txtMsg.Text);
            }
            catch (Exception ex)
            {
                LogHelper.Error("解析处理MQ消息失败：" + ex);
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="text"></param>
        public static void ParseXml(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            XElement doc = XElement.Parse(text.Trim());

            #region KEEPALIVE消息

            if (StringUtils.EqualsEx(doc.Name.LocalName,"keepalive"))
            {
                LogHelper.Debug("收到 KeepAlive消息："+doc.Value);
                //KEEPALIVE信息
                return;
            }
            #endregion

            #region MSG消息
            if (StringUtils.EqualsEx(doc.Name.LocalName, "msg"))
            {
                var meta = doc.Element("META");
                string msgType = meta.GetValue("TYPE");//消息类型
                string subType = meta.GetValue("SUBTYPE");//消息子类型

                LogHelper.Info("接收到消息类型:{0},子类型:{1}.".FormatWith(msgType,subType));

                #region 动态航班添加 修改

                if (StringUtils.EqualsEx(msgType, "DYNFLIGHT") && (StringUtils.EqualsEx(subType, "ADD") || StringUtils.EqualsEx(subType, "MODIFY")))
                {
                    //添加动态航班
                    XElement dflt = doc.Element("DFLT");
                    Flight flight = new Flight();
                    //航班数
                    flight.Id = Guid.NewGuid();//主键
                    flight.Flag = "Normal";//正常航班
                    flight.FlightId = dflt.GetValue("ID");//航班id 集成的标志 如： 558169
                    flight.FlightNo = dflt.GetValue("FLIGHTNO");//航班号  4912
                    flight.FlightDate = DateTime.ParseExact(dflt.GetValue("FLIGHTDATE"), "yyyyMMdd", CultureInfo.InvariantCulture); ;//航班日期 如10150923
                    flight.AirlineIata = dflt.GetValue("AIRLINE");//航空公司二字码 如:SC
                    flight.InOut = dflt.GetValue("ISARRFLIGHT") == "1" ? "I" : "O";//1是进港，0是出港
                    flight.Task = dflt.GetValue("TASK");//航班任务 如 W/Z
                    string regionId = dflt.GetValue("REGIONID");//区域属性 100001 DMST国内; INTER 100002 国际; ZONE 100003 地区; 100004 MIX混合
                    switch (regionId)
                    {
                        case "100001":
                            flight.Region = "FD";
                            break;
                        case "100002":
                            flight.Region = "FI";
                            break;
                        case "100003":
                            flight.Region = "FA";
                            break;
                        case "100004":
                            flight.Region = "FF";
                            break;
                    }

                    flight.CraftNo = dflt.GetValue("CRAFT");//机号
                    flight.CraftType = dflt.GetValue("CRAFTMODEL");//机型

                    flight.Status = dflt.GetValue("STATUS");//航班状态  LBD	催促登机 POK	登机截止 DEP	起飞	  TBR	过站登机 BOR	本站登机 CKO	值机截止 CKI	值机开始 ARR	到达	   ONR	前方起飞
                    flight.ExStatus = dflt.GetValue("ABNSTATUS");//异常航班状态代码 CAN	取消	   ALT	备降	   RTN	返航	   DLY	延误
                    string abnrsn = dflt.GetValue("ABNRSN");//航班异常原因 如：前站天气

                    string alterStation = null;//变更站 备降
                    DateTime? planStartTime = null;//计划开始
                    DateTime? planStopTime = null;//计划结束
                    DateTime? alterStartTime = null;//变更开始
                    DateTime? alterStopTime = null;//变更结束
                    DateTime? realStartTime = null;//实际开始
                    DateTime? realStopTime = null;//实际结束

                    #region 经由
                    if (dflt.Element("ROUTELST") != null)
                    {
                        var routeList = dflt.Element("ROUTELST").Elements("ROUTE").ToList(); //航站列表
                        for (int i = 0; i < routeList.Count(); i++)
                        {
                            if (i == 0)
                            {
                                //出发站
                                flight.Departure = routeList[i].GetValue("AIRPORTIATA"); //出发站
                                routeList[i].Update(ref planStartTime, "PLANTAKEOFF"); //计划开始
                                routeList[i].Update(ref alterStartTime, "ALTERTAKEOFF");
                                routeList[i].Update(ref realStartTime, "REALTAKEOFF");
                            }
                            else if (i == routeList.Count - 1 && routeList.Count >= 2)
                            {
                                //终点站
                                flight.Destination = routeList[i].GetValue("AIRPORTIATA");
                                routeList[i].Update(ref planStopTime, "PLANLANDING");
                                routeList[i].Update(ref alterStopTime, "ALTERLANDING");
                                routeList[i].Update(ref realStopTime, "REALLANDING");
                            }
                            else
                            {
                                //经由
                                FlightVia via = new FlightVia();
                                via.FlightId = flight.Id;
                                via.AirportIata = routeList[i].GetValue("AIRPORTIATA");
                                via.StartTime = routeList[i].GetTime("REALTAKEOFF") ??
                                                routeList[i].GetTime("ALTERTAKEOFF") ??
                                                routeList[i].GetTime("PLANTAKEOFF");
                                via.StopTime = routeList[i].GetTime("REALPLANDING") ??
                                               routeList[i].GetTime("ALTERLANDING") ??
                                               routeList[i].GetTime("PLANLANDING");

                                flight.FlightVias.Add(via);
                            }

                            if (routeList[i].GetValue("ISALTERAIRPORT") == "1")
                            {
                                //备降站 取第一个
                                alterStation = alterStation ?? flight.Departure;
                            }
                        }

                        flight.PlanStartTime = planStartTime;
                        flight.PlanStopTime = planStopTime;
                        flight.AlterStartTime = alterStartTime;
                        flight.AlterStopTime = alterStopTime;
                        flight.RealStartTime = realStartTime;
                        flight.RealStopTime = realStopTime;
                        flight.AlterStation = alterStation;
                    }
                    #endregion

                    #region 共享航班

                    if (dflt.Element("SHARELST") != null)
                    {
                        var shareList = dflt.Element("SHARELST").Elements("SHARE").ToList();
                        foreach (var share in shareList)
                        {
                            FlightShare flightShare = new FlightShare()
                            {
                                FlightId = flight.Id,
                                AirlineIata = share.GetValue("AIRLINE"),
                                FlightNo = share.GetValue("FLIGHTNO")
                            };
                            flight.FlightShares.Add(flightShare);
                        }
                    }

                    #endregion

                    #region 值机柜台

                    if (dflt.Element("CKITIME") != null)
                    {
                        //值机柜台时间
                        var ckiTime = dflt.Element("CKITIME");
                        flight.StartCheckinTime = ckiTime.GetTime("REALOPENTIME") ?? ckiTime.GetTime("PLANOPENTIME");
                        flight.StopCheckinTime = ckiTime.GetTime("REALENDTIME") ?? ckiTime.GetTime("PLANENDTIME");
                    }

                    if (dflt.Element("CKICLST") != null)
                    {
                        //值机柜台
                        var ckiList = dflt.Element("CKICLST").Elements("CKIC").ToList();
                        HashSet<string> counterDisplay = new HashSet<string>();
                        foreach (var check in ckiList)
                        {
                            string checkName = check.GetValue("CHICNAME");
                            if (string.IsNullOrEmpty(checkName))
                            {
                                continue;
                            }
                            FlightInfrastructure flightInfrastructure = new FlightInfrastructure()
                            {
                                FlightId = flight.Id,
                                InfrastructureType = "Counter",
                                InfrastructureCode = checkName,
                                StartTime = flight.StartCheckinTime,
                                StopTime = flight.StopCheckinTime,
                                Flag = "N"
                            };
                            string ckicAreaCode = check.GetValue("CKICAREACODE");
                            if (!string.IsNullOrEmpty(ckicAreaCode))
                            {
                                counterDisplay.Add(ckicAreaCode);
                            }
                            flight.FlightInfrastructures.Add(flightInfrastructure);
                        }
                        flight.CounterDisplay = string.Empty;
                        foreach (var item in counterDisplay)
                        {
                            flight.CounterDisplay += item + ",";
                        }
                        flight.CounterDisplay = flight.CounterDisplay.TrimEnd(',');
                    }

                    #endregion

                    #region 登机口

                    if (dflt.Element("GATETIME") != null)
                    {
                        var gateTime = dflt.Element("GATETIME");
                        flight.StartBoardTime = gateTime.GetTime("REALOPENTIME") ?? gateTime.GetTime("PLANOPENTIME");
                        flight.StopBoardTime = gateTime.GetTime("REALENDTIME") ?? gateTime.GetTime("PLANENDTIME");
                    }

                    if (dflt.Element("GATELST") != null)
                    {
                        var gateList = dflt.Element("GATELST").Elements("GATE");
                        HashSet<string> gateDisplay = new HashSet<string>();
                        foreach (var gate in gateList)
                        {
                            string gateName = gate.GetValue("GATENAME");

                            if (string.IsNullOrEmpty(gateName))
                            {
                                continue;
                            }

                            FlightInfrastructure flightInfrastructure = new FlightInfrastructure()
                            {
                                FlightId = flight.Id,
                                InfrastructureType = "Gate",
                                InfrastructureCode = gateName,
                                StartTime = flight.StartBoardTime,
                                StopTime = flight.StopBoardTime,
                                Flag="N"
                            };

                            flight.FlightInfrastructures.Add(flightInfrastructure);
                            gateDisplay.Add(gateName);
                        }

                        flight.GateDisplay = string.Empty;
                        foreach (var item in gateDisplay)
                        {
                            flight.GateDisplay += item + ",";
                        }
                        flight.GateDisplay = flight.GateDisplay.TrimEnd(',');
                    }


                    #endregion

                    #region 行李转盘

                    if (flight.InOut == "I")
                    {
                        //进港行李转盘
                        if (dflt.Element("ACRSLTIME") != null)
                        {
                            var claimTime = dflt.Element("ACRSLTIME");
                            flight.StartClaimTime = claimTime.GetTime("REALOPENTIME") ??
                                                    claimTime.GetTime("PLANOPENTIME");
                            flight.StopClaimTime = claimTime.GetTime("REALENDTIME") ?? claimTime.GetTime("PLANENDTIME");

                        }

                        if (dflt.Element("ACRSLLST") != null)
                        {
                            var claimList = dflt.Element("ACRSLLST").Elements("ACRSL").ToList();
                            HashSet<string> claimDisplay = new HashSet<string>();
                            foreach (var claim in claimList)
                            {
                                string claimName = claim.GetValue("ACRSLNAME");
                                if (string.IsNullOrEmpty(claimName))
                                {
                                    continue;
                                }
                                FlightInfrastructure flightInfrastructure = new FlightInfrastructure()
                                {
                                    FlightId = flight.Id,
                                    StartTime = flight.StartClaimTime,
                                    StopTime = flight.StopClaimTime,
                                    Flag = "N",
                                    InfrastructureType = "Claim",
                                    InfrastructureCode = claimName
                                };
                                flight.FlightInfrastructures.Add(flightInfrastructure);
                                claimDisplay.Add(claimName);
                            }

                            flight.ClaimDisplay = string.Empty;
                            foreach (var item in claimDisplay)
                            {
                                flight.ClaimDisplay += item + ",";
                            }

                            flight.ClaimDisplay = flight.ClaimDisplay.TrimEnd(',');
                        }
                    }
                    else
                    {
                        //出港行李转盘
                        if (dflt.Element("DCRSLTIME") != null)
                        {
                            var claimTime = dflt.Element("DCRSLTIME");
                            flight.StartClaimTime = claimTime.GetTime("REALOPENTIME") ??
                                                    claimTime.GetTime("PLANOPENTIME");
                            flight.StopClaimTime = claimTime.GetTime("REALENDTIME") ?? claimTime.GetTime("PLANENDTIME");

                        }

                        if (dflt.Element("DCRSLLST") != null)
                        {
                            var claimList = dflt.Element("DCRSLLST").Elements("DCRSL").ToList();
                            HashSet<string> claimDisplay = new HashSet<string>();
                            foreach (var claim in claimList)
                            {
                                string claimName = claim.GetValue("DCRSLNAME");
                                if (string.IsNullOrEmpty(claimName))
                                {
                                    continue;
                                }
                                FlightInfrastructure flightInfrastructure = new FlightInfrastructure()
                                {
                                    FlightId = flight.Id,
                                    StartTime = flight.StartClaimTime,
                                    StopTime = flight.StopClaimTime,
                                    Flag = "N",
                                    InfrastructureType = "Claim",
                                    InfrastructureCode = claimName
                                };
                                flight.FlightInfrastructures.Add(flightInfrastructure);
                                claimDisplay.Add(claimName);
                            }

                            flight.ClaimDisplay = string.Empty;
                            foreach (var item in claimDisplay)
                            {
                                flight.ClaimDisplay += item + ",";
                            }

                            flight.ClaimDisplay = flight.ClaimDisplay.TrimEnd(',');
                        }
                    }

                    #endregion

                    #region 机位

                    if (dflt.Element("SEATTIME") != null)
                    {
                        var standTime = dflt.Element("SEATTIME");
                        flight.StartStandTime = standTime.GetTime("REALARVTIME") ?? standTime.GetTime("PLANARVTIME");
                            //进机位时间 有实际显实际 否则显计划
                        flight.StopStandTime = standTime.GetTime("REALDEPTIME") ?? standTime.GetTime("PLANDEPTIME");
                            //出机位时间  有实际显实际 否则显计划
                    }

                    if (dflt.Element("SEATLST") != null)
                    {
                        var standList = dflt.Element("SEATLST").Elements("SEAT").ToList();
                        HashSet<string> standDisplay = new HashSet<string>();

                        foreach (var stand in standList)
                        {
                            string standName = stand.GetValue("SEATNAME");
                            if (string.IsNullOrEmpty(standName))
                            {
                                continue;
                            }

                            FlightInfrastructure flightInfrastructure = new FlightInfrastructure()
                            {
                                FlightId = flight.Id,
                                Flag = "N",
                                InfrastructureCode = standName,
                                InfrastructureType = "Stand",
                                StartTime = flight.StartBoardTime,
                                StopTime = flight.StopBoardTime
                            };
                            flight.FlightInfrastructures.Add(flightInfrastructure);
                            standDisplay.Add(standName);
                        }

                        flight.StandDisplay = string.Empty;
                        foreach (var item in standDisplay)
                        {
                            flight.StandDisplay += item + ",";
                        }
                        flight.StandDisplay = flight.StandDisplay.TrimEnd(',');
                    }

                    #endregion

                    #region 保存数据

                    using (SyncContext context = new SyncContext())
                    {
                        //以航班号+航班日期+进出港为标记
                        var oldFlight = context.Flights.Where(r => (r.FlightId == flight.FlightId ||
                                 (r.AirlineIata == flight.AirlineIata && r.FlightNo == flight.FlightNo &&
                                  r.InOut == flight.InOut &&
                                  SqlFunctions.DateDiff("day", r.FlightDate, flight.FlightDate) == 0))).Include(r=>r.FlightVias).Include(r=>r.FlightShares).Include(r=>r.FlightInfrastructures).FirstOrDefault();

                        if (oldFlight != null)
                        {
                            if (StringUtils.EqualsEx(oldFlight.Flag, "Manual"))
                            {
                                //手工航班
                                return;
                            }

                            //更新状态时间
                            if (oldFlight.Status != flight.Status && !string.IsNullOrEmpty(flight.Status))
                            {
                                flight.StatusTime = DateTime.Now;
                            }

                            Update(oldFlight, flight);
                            int count = context.SaveChanges();
                            LogHelper.Info("计划修改1条航班，影响数据{0}行，操作类型：{1}，子类型{2}".FormatWith(count, msgType, subType));
                        }
                        else
                        {
                            //如果历史中包含，则不添加
                            var historyFlight = context.HistoryFlights.FirstOrDefault(r => r.FlightId == flight.FlightId && r.FlightNo == flight.FlightNo && r.AirlineIata == flight.AirlineIata && SqlFunctions.DateDiff("day",r.FlightDate,flight.FlightDate)<=1);
                            if (historyFlight != null)
                            {
                                LogHelper.Info("数据已经转历史，不执行添加操作，操作类型：{0}，子类型{1}".FormatWith( msgType, subType));
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(flight.Status))
                                {
                                    flight.StatusTime = DateTime.Now;
                                }
                                context.Flights.Add(flight);
                                int count = context.SaveChanges();
                                LogHelper.Info("计划新增1条航班，影响数据{0}行，操作类型：{1}，子类型{2}".FormatWith(count, msgType, subType));
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region 动态航班删除

                if (StringUtils.EqualsEx(msgType, "DYNFLIGHT") && StringUtils.EqualsEx(subType, "DELETE"))
                {
                    XElement dflt = doc.Element("DFLT");
                    string id = dflt.GetValue("ID");
                    if (string.IsNullOrEmpty(id))
                    {
                        LogHelper.Info("没有删除任何数据");
                    }
                    else
                    {
                        using (SyncContext context = new SyncContext())
                        {
                            var fltRemove = context.Flights.Where(r => r.FlightId == id).Include(r=>r.FlightVias).Include(r=>r.FlightInfrastructures).Include(r=>r.FlightShares).ToList();
                            if (fltRemove.Count > 0)
                            {
                                context.Flights.RemoveRange(fltRemove);
                                int count = context.SaveChanges();
                                LogHelper.Info("计划删除{0}条航班，操作类型：{1}，子类型：{2},影像数据{3}行。".FormatWith(fltRemove.Count, msgType, subType,count));
                            }
                            else
                            {
                                LogHelper.Info("没有获取到匹配id={0}的数据，没有删除任何数据".FormatWith(id));
                            }
                        }
                    }
                    
                }

                #endregion

                return;
            }
            #endregion

            LogHelper.Warn("该消息未被识别，未被处理");
        }

        /// <summary>
        /// 更新航班
        /// </summary>
        /// <param name="oldFlight"></param>
        /// <param name="flight"></param>
        private static void Update(Flight oldFlight, Flight flight)
        {
            if (oldFlight == null || flight == null)
            {
                return;
            }

            #region 更新字段
            if (oldFlight.FlightId != flight.FlightId)
            {
                oldFlight.FlightId = flight.FlightId;
            }

            if (oldFlight.FlightNo != flight.FlightNo)
            {
                oldFlight.FlightNo = flight.FlightNo;
            }

            if (oldFlight.FlightDate != flight.FlightDate)
            {
                oldFlight.FlightDate = flight.FlightDate;
            }

            if (oldFlight.AirlineIata != flight.AirlineIata)
            {
                oldFlight.AirlineIata = flight.AirlineIata;
            }

            if (oldFlight.CraftType != flight.CraftType)
            {
                oldFlight.CraftType = flight.CraftType;
            }
            if (oldFlight.CraftNo != flight.CraftNo)
            {
                oldFlight.CraftNo = flight.CraftNo;
            }

            if (oldFlight.InOut != flight.InOut)
            {
                oldFlight.InOut = flight.InOut;
            }

            if (oldFlight.Departure != flight.Departure)
            {
                oldFlight.Departure = flight.Departure;
            }

            if (oldFlight.Destination != flight.Destination)
            {
                oldFlight.Destination = flight.Destination;
            }

            if (oldFlight.AlterStation != flight.AlterStation)
            {
                oldFlight.AlterStation = flight.AlterStation;
            }

            if (oldFlight.PlanStartTime != flight.PlanStartTime && flight.PlanStartTime.HasValue)
            {
                oldFlight.PlanStartTime = flight.PlanStartTime;
            }

            if (oldFlight.PlanStopTime != flight.PlanStopTime && flight.PlanStopTime.HasValue)
            {
                oldFlight.PlanStopTime = flight.PlanStopTime;
            }

            if (oldFlight.AlterStartTime != flight.AlterStartTime && flight.AlterStartTime.HasValue)
            {
                oldFlight.AlterStartTime = flight.AlterStartTime;
            }

            if (oldFlight.AlterStopTime != flight.AlterStopTime && flight.AlterStopTime.HasValue)
            {
                oldFlight.AlterStopTime = flight.AlterStopTime;
            }

            if (oldFlight.RealStartTime != flight.RealStartTime && flight.RealStartTime.HasValue)
            {
                oldFlight.RealStartTime = flight.RealStartTime;
            }

            if (oldFlight.RealStopTime != flight.RealStopTime && flight.RealStopTime.HasValue)
            {
                oldFlight.RealStopTime = flight.RealStopTime;
            }

            if (oldFlight.Status != flight.Status)
            {
                oldFlight.Status = flight.Status;
            }

            if (oldFlight.StatusTime != flight.StatusTime && flight.StatusTime.HasValue)
            {
                oldFlight.StatusTime = flight.StatusTime;
            }

            if (oldFlight.ExStatus != flight.ExStatus)
            {
                oldFlight.ExStatus = flight.ExStatus;
            }

            if (oldFlight.ExStatusReason != flight.ExStatusReason)
            {
                oldFlight.ExStatusReason = flight.ExStatusReason;
            }

            if (oldFlight.GateDisplay != flight.GateDisplay)
            {
                oldFlight.GateDisplay = flight.GateDisplay;
            }

            if (oldFlight.CounterDisplay != flight.CounterDisplay)
            {
                oldFlight.CounterDisplay = flight.CounterDisplay;
            }

            if (oldFlight.StandDisplay != flight.StandDisplay)
            {
                oldFlight.StandDisplay = flight.StandDisplay;
            }

            if (oldFlight.ClaimDisplay != flight.ClaimDisplay)
            {
                oldFlight.ClaimDisplay = flight.ClaimDisplay;
            }

            if (oldFlight.Region != flight.Region)
            {
                oldFlight.Region = flight.Region;
            }

            if (oldFlight.Task != flight.Task)
            {
                oldFlight.Task = flight.Task;
            }

            if (oldFlight.StartCheckinTime != flight.StartCheckinTime && flight.StartCheckinTime.HasValue)
            {
                oldFlight.StartCheckinTime = flight.StartCheckinTime;
            }

            if (oldFlight.StopCheckinTime != flight.StopCheckinTime && flight.StopCheckinTime.HasValue)
            {
                oldFlight.StopCheckinTime = flight.StopCheckinTime;
            }

            if (oldFlight.StartBoardTime != flight.StartBoardTime && flight.StartBoardTime.HasValue)
            {
                oldFlight.StartBoardTime = flight.StartBoardTime;
            }

            if (oldFlight.StopBoardTime != flight.StopBoardTime && flight.StopBoardTime.HasValue)
            {
                oldFlight.StopBoardTime = flight.StopBoardTime;
            }

            if (oldFlight.StartClaimTime != flight.StartClaimTime && flight.StartClaimTime.HasValue)
            {
                oldFlight.StartClaimTime = flight.StartClaimTime;
            }

            if (oldFlight.StopClaimTime != flight.StopClaimTime && flight.StopClaimTime.HasValue)
            {
                oldFlight.StopClaimTime = flight.StopClaimTime;
            }

            if (oldFlight.StartStandTime != flight.StartStandTime && flight.StartStandTime.HasValue)
            {
                oldFlight.StartStandTime = flight.StartStandTime;
            }

            if (oldFlight.StopStandTime != flight.StopStandTime && flight.StopStandTime.HasValue)
            {
                oldFlight.StopStandTime = flight.StopStandTime;
            }
            #endregion

            #region 更新关联表

            #region 更新经停
            //需要移除的经由
            var removeVias = oldFlight.FlightVias.Where(r => flight.FlightVias.All(m => m.AirportIata != r.AirportIata)).ToList();
            //需要添加的经由
            var addVias =
                flight.FlightVias.Where(r => oldFlight.FlightVias.All(m => m.AirportIata != r.AirportIata)).ToList();

            foreach (var item in removeVias)
            {
                oldFlight.FlightVias.Remove(item);
            }

            //现存的via
            foreach (var item in oldFlight.FlightVias)
            {
                var newItem = flight.FlightVias.FirstOrDefault(r => r.AirportIata == item.AirportIata);
                if (newItem != null)
                {
                    if (newItem.StartTime != item.StartTime)
                    {
                        item.StartTime = newItem.StartTime;
                    }
                    if (newItem.StopTime != item.StopTime)
                    {
                        item.StopTime = newItem.StopTime;
                    }
                }
            }
            //新增的
            foreach (var item in addVias)
            {
                item.FlightId = oldFlight.Id;
                oldFlight.FlightVias.Add(item);
            }
            #endregion

            #region 更新共享航班

            //需要移除的共享
            var removeShares = oldFlight.FlightShares.Where(r => flight.FlightShares.All(m => m.AirlineIata != r.AirlineIata || m.FlightNo !=r.FlightNo)).ToList();
            //需要添加的共享
            var addShares =
                flight.FlightShares.Where(r => oldFlight.FlightShares.All(m => m.AirlineIata != r.AirlineIata || m.FlightNo != r.FlightNo)).ToList();

            foreach (var item in removeShares)
            {
                oldFlight.FlightShares.Remove(item);
            }

            //新增的
            foreach (var item in addShares)
            {
                item.FlightId = oldFlight.Id;
                oldFlight.FlightShares.Add(item);
            }

            #endregion

            #region 更新航班设施
            //需要移除的
            var removeInfrastructures = oldFlight.FlightInfrastructures.Where(r => !flight.FlightInfrastructures.Any(m => m.InfrastructureCode == r.InfrastructureCode && m.InfrastructureType ==r.InfrastructureType )).ToList();
            //需要添加的
            var addInfrastructures =
                flight.FlightInfrastructures.Where(r => !oldFlight.FlightInfrastructures.Any(m => m.InfrastructureCode == r.InfrastructureCode && m.InfrastructureType ==r.InfrastructureType)).ToList();

            foreach (var item in removeInfrastructures)
            {
                item.Flag = "O";//旧的设施
            }

            //现存的via
            foreach (var item in oldFlight.FlightInfrastructures)
            {
                var newItem = flight.FlightInfrastructures.FirstOrDefault(m => m.InfrastructureCode == item.InfrastructureCode && m.InfrastructureType == item.InfrastructureType);
                if (newItem != null)
                {
                    if (newItem.StartTime != item.StartTime)
                    {
                        item.StartTime = newItem.StartTime;
                    }
                    if (newItem.StopTime != item.StopTime)
                    {
                        item.StopTime = newItem.StopTime;
                    }

                    if (item.Flag != "N")
                    {
                        item.Flag = "N";
                    }
                }
            }
            //新增的
            foreach (var item in addInfrastructures)
            {
                item.FlightId = oldFlight.Id;
                oldFlight.FlightInfrastructures.Add(item);
            }
            #endregion

            #endregion

        }


        /// <summary>
        /// 连接断开
        /// </summary>
        private static void connection_ConnectionInterruptedListener()
        {
            isAlive = false;
            LogHelper.Error("ConnectionInterruptedListener连接发生异常连接断开");
        }

        /// <summary>
        /// 连接异常
        /// </summary>
        /// <param name="exception"></param>
        private static void connection_ExceptionListener(Exception exception)
        {
            isAlive = false;
            LogHelper.Error("connection_ExceptionListener连接发生异常：" + exception);
        }


        /// <summary>
        /// 关闭消费者
        /// </summary>
        public static void CloseConsumer()
        {
            try
            {

                LogHelper.Info("开始关闭MQ:"+ConfigUtils.GetString(MQUri));

                if (consumer != null)
                {
                    consumer.Dispose();
                    consumer.Close();
                }
                consumer = null;

                if (session != null)
                {
                    session.Dispose();
                    session.Close();
                }
                session = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("关闭MQ Session失败："+ex);
            }

            try
            {
                if (connection != null)
                {
                    connection.Stop();
                    connection.Dispose();
                    connection.Close();
                }
                connection = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("关闭MQ连接失败：" + ex);
            }

            isAlive = false;
        }

        /// <summary>
        /// MQ是否存活
        /// </summary>
        /// <returns></returns>
        public static Boolean IsAlive()
        {
            return isAlive;
        }

    }
}
