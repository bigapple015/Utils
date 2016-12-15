using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmbInterface.Utils;
using IBM.WMQ;

namespace AmbInterface.Models
{
    /// <summary>
    /// IBM MQ 适配器
    /// </summary>
    public class MQAdapter
    {
        #region 配置相关

        /// <summary>
        /// MQ Queue Manager的名字 MQ队列管理器的名字
        /// </summary>
        private static readonly string queueManagerName = ConfigUtils.GetString("MQManagerName");

        /// <summary>
        /// 通道名字  SVRCONN
        /// </summary>
        private static readonly string channelName = ConfigUtils.GetString("ChannelName");

        /// <summary>
        /// 连接名字  采用 hostIpOrName(Port)格式
        /// </summary>
        private static readonly string connName = ConfigUtils.GetString("ConnName");

        /// <summary>
        /// 行李接收队列的名字
        /// </summary>
        private static readonly string XlRecvQueueName = ConfigUtils.GetString("XlRecvQueueName");

        /// <summary>
        /// 安检接收队列的名字
        /// </summary>
        private static readonly string AjRecvQueueName = ConfigUtils.GetString("AjRecvQueueName");

        ///// <summary>
        ///// the time you want a thread to wait on empty queue until a relevant message show up
        ///// </summary>
        //private static readonly int WaitInterval = ConfigUtils.GetInteger("WaitInterval", 20000);

        ///// <summary>
        ///// 消息超时时间 5分钟
        ///// </summary>
        //private static readonly int ExpirySeconds = 300;

        /// <summary>
        /// 是否连接
        /// </summary>
        private static volatile bool isConnected = false;

        #endregion

        #region 属性
        
        /// <summary>
        /// MQ 队列管理器
        /// </summary>
        private static volatile MQQueueManager mqManager = null;

        /// <summary>
        /// 行李队列
        /// </summary>
        private static volatile MQQueue XlRecvQueue = null;

        /// <summary>
        /// 安检接受队列
        /// </summary>
        private static volatile MQQueue AjRecvQueue = null;

        /// <summary>
        /// 所有放入到MQ队列中的消息，都采用缺省的字符集Unicode,其编码字符集标示为1200。
        /// </summary>
        private const int CharacterSetCode = 1200;

        /// <summary>
        /// 安检接收线程
        /// </summary>
        private static Thread recvThread = null;

        /// <summary>
        /// 行李接收线程
        /// </summary>
        private static Thread xlRecvThread = null;

        /// <summary>
        /// 监控定时器
        /// </summary>
        private static volatile MonitorTimer monitorTimer;

        /// <summary>
        /// 出错次数
        /// </summary>
        private static volatile int errorCount = 0;
        

        #endregion

        #region 方法

        /// <summary>
        /// 初始化时调用一次
        /// </summary>
        public static void Init()
        {
            if (monitorTimer != null)
            {
                return;
            }

            monitorTimer = new MonitorTimer(state =>
            {
                if (!IsConnected())
                {
                    Start();
                }
                else
                {
                    NLogHelper.Trace("MQ连接正常");
                }
            });

            recvThread = new Thread(RecvThreadCallBack);

            recvThread.Start();

            xlRecvThread = new Thread(XlRecvThreadCallBack);
            xlRecvThread.Start();

            MessageHandler.Init();
        }

        /// <summary>
        /// 多线程回调
        /// </summary>
        private static void RecvThreadCallBack()
        {
            while (true)
            {
                try
                {
                    if (mqManager == null || AjRecvQueue == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    

                    RecvMessage(MessageHandler.AddToQueue);

                }
                catch (Exception ex)
                {
                    NLogHelper.Error("接受MQ消息失败："+ex);
                    SafeInvoke.Safe(()=> {Thread.Sleep(1000);});
                }
            }
        }

        /// <summary>
        /// 行李多线程回调
        /// </summary>
        private static void XlRecvThreadCallBack()
        {
            while (true)
            {
                try
                {
                    if (mqManager == null || XlRecvQueue == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }


                    RecvXlMessage(MessageHandler.AddToXlQueue);

                }
                catch (Exception ex)
                {
                    NLogHelper.Error("接受行李MQ消息失败：" + ex);
                    SafeInvoke.Safe(() => { Thread.Sleep(1000); });
                }
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        private static bool Start()
        {
            try
            {
                ShutDown();
               
                //创建一个客户端连接
                mqManager = new MQQueueManager(queueManagerName,channelName,connName);
                //指定想要打开的队列
                //XlRecvQueue = mqManager.AccessQueue(XlRecvQueueName,MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING/*停顿 停止*/ | MQC.MQOO_INQUIRE/*询问*/);
                XlRecvQueue = mqManager.AccessQueue(XlRecvQueueName, MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING /*停顿 停止*/| MQC.MQOO_INQUIRE /*询问*/);
                /*
                 * MQC.MQOO_INPUT_AS_Q_DEF Open to get messages using queue-defined default.
                 * MQC.MQOO_FAIL_IF_QUIESCING Fail if the queue manager is quiescing.
                 * MQC.MQOO_INQUIRE Open for inquiry - required if you want to query properties.
                 */
                //连接队列
                AjRecvQueue = mqManager.AccessQueue(AjRecvQueueName,MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING /*停顿 停止*/| MQC.MQOO_INQUIRE /*询问*/);
                NLogHelper.Info("初始化mq队列成功");
                isConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("初始化队列失败："+ex);
                isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private static void ShutDown()
        {

            if (XlRecvQueue != null)
            {
                SafeInvoke.Safe(() =>
                {
                    XlRecvQueue.Close();
                }, ex =>
                {
                    NLogHelper.Error("关闭XlRecvQueue失败：" + ex);
                });

                XlRecvQueue = null;
            }

            if (AjRecvQueue != null)
            {
                SafeInvoke.Safe(() =>
                {
                    AjRecvQueue.Close();
                }, ex =>
                {
                    NLogHelper.Error("关闭XjRecvQueue失败：" + ex);
                });
                AjRecvQueue = null;
            }

            if (mqManager != null)
            {
                SafeInvoke.Safe(() =>
                {
                    mqManager.Disconnect();
                    mqManager.Close();
                }, ex =>
                {
                    NLogHelper.Error("关闭队列管理器出错：" + ex);
                });
                mqManager = null;
            }
        }

        ///// <summary>
        ///// 发送消息
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="exceptionHandler"></param>
        //public static void SendMessage(string message, Action<Exception> exceptionHandler = null)
        //{
        //    if (string.IsNullOrWhiteSpace(message))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        //定义要发送的消息
        //        MQMessage sendMessage  = new MQMessage();
        //        sendMessage.WriteString(message);
        //        sendMessage.CharacterSet = CharacterSetCode;
        //        sendMessage.MessageType = MQC.MQMT_DATAGRAM;
        //        //This is expressed in units of tenths of a second. 单位十分之一秒
        //        sendMessage.Expiry = ExpirySeconds*10;
        //        //消息选项
        //        MQPutMessageOptions putOptions = new MQPutMessageOptions();
        //        putOptions.Options = MQC.MQGMO_FAIL_IF_QUIESCING;
        //        //将消息放入队列
        //        XlRecvQueue.Put(sendMessage,putOptions);
                
        //    }

        //    catch (Exception ex)
        //    {
        //        if (exceptionHandler != null)
        //        {
        //            exceptionHandler(ex);
        //        }
        //    }
            
        //}

            /// <summary>
            /// 上一次接收时间
            /// </summary>
        private static DateTime LastReceiveTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="messageHandler">接收到消息的消息处理器</param>
        public static void RecvMessage(Action<string> messageHandler = null)
        {
            try
            {
                //接收配置
                MQGetMessageOptions getOptions = new MQGetMessageOptions();

                //等待读取 Wait for a message to arrive.
                //getOptions.Options += MQC.MQGMO_WAIT | MQC.MQGMO_FAIL_IF_QUIESCING;
                getOptions.Options = MQC.MQGMO_WAIT | MQC.MQGMO_FAIL_IF_QUIESCING;
                //WaitInterval is the maximum time in milliseconds that an MQQueue.get call waits for a suitable message to arrive. 
                //getOptions.WaitInterval = WaitInterval;

                //接收消息
                MQMessage recvMessage = new MQMessage();
                recvMessage.CharacterSet = CharacterSetCode;
                recvMessage.MessageType = MQC.MQMT_DATAGRAM;
                //接收消息
                AjRecvQueue.Get(recvMessage, getOptions);
                
                //读取消息
                string message = recvMessage.ReadString(recvMessage.MessageLength);

                if (messageHandler != null)
                {
                    messageHandler(message);
                }
            }
            catch (MQException ex)
            {
                //代码的具体含义，参见 Application Program reference P336
                if (ex.Reason == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    if ((DateTime.Now - LastReceiveTime).TotalSeconds > 22)
                    {
                        LastReceiveTime = DateTime.Now;
                        NLogHelper.Debug("没有消息可用");
                    }
                    Thread.Sleep(5);
                    //NLogHelper.Debug("没有消息可用");
                }
                else if (ex.Reason == MQC.MQRC_CONNECTION_BROKEN)
                {
                    NLogHelper.Warn("连接断开");
                    
                    isConnected = false;
                    Thread.Sleep(500);
                }
                else
                {
                    errorCount++;

                    if (errorCount > 10000)
                    {
                        errorCount = 1;
                    }

                    if (errorCount%10 == 0)
                    {
                        //连续10次错误重启
                        isConnected = false;
                        Thread.Sleep(500);
                    }

                    
                    throw ex;
                }

            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="messageHandler">接收到消息的消息处理器</param>
        public static void RecvXlMessage(Action<string> messageHandler = null)
        {
            try
            {
                //接收配置
                MQGetMessageOptions getOptions = new MQGetMessageOptions();

                ////等待读取 Wait for a message to arrive.
                //getOptions.Options += MQC.MQGMO_WAIT | MQC.MQGMO_FAIL_IF_QUIESCING ;
                ////WaitInterval is the maximum time in milliseconds that an MQQueue.get call waits for a suitable message to arrive. 
                //getOptions.WaitInterval = WaitInterval;
                getOptions.Options = MQC.MQGMO_WAIT | MQC.MQGMO_FAIL_IF_QUIESCING;

                //接收消息
                MQMessage recvMessage = new MQMessage();
                recvMessage.CharacterSet = CharacterSetCode;
                recvMessage.MessageType = MQC.MQMT_DATAGRAM;
                //接收消息
                XlRecvQueue.Get(recvMessage, getOptions);

                //读取消息
                string message = recvMessage.ReadString(recvMessage.MessageLength);

                if (messageHandler != null)
                {
                    messageHandler(message);
                }
            }
            catch (MQException ex)
            {
                //代码的具体含义，参见 Application Program reference P336
                if (ex.Reason == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    Thread.Sleep(5);
                }
                else if (ex.Reason == MQC.MQRC_CONNECTION_BROKEN)
                {
                    NLogHelper.Warn("行李消息连接断开");

                    isConnected = false;
                    Thread.Sleep(500);
                }
                else
                {
                    errorCount++;

                    if (errorCount > 10000)
                    {
                        errorCount = 1;
                    }

                    if (errorCount % 10 == 0)
                    {
                        //连续10次错误重启
                        isConnected = false;
                        Thread.Sleep(500);
                    }


                    throw ex;
                }

            }
        }


        /// <summary>
        /// 检测是否连接
        /// </summary>
        /// <returns></returns>
        private static bool IsConnected()
        {
            try
            {
                if (mqManager == null || AjRecvQueue == null || XlRecvQueue == null)
                {
                    return false;
                }

                if (!isConnected)
                {
                    return false;
                }
                return mqManager.IsConnected;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("检测是否连接错误："+ex);
                return false;
            }
        }

        #endregion


    }
}
