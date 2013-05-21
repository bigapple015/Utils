using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using iFlyTek.ECSS30.Tool.SMSProc;
using System.Data;
using System.IO;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// Excel数据源过滤器
    /// </summary>
    public class ExcelFilter
    {
        /// <summary>
        /// 所有短信的条数
        /// </summary>
        public static int SumCount = 0;

        /// <summary>
        /// 识别短信总数
        /// </summary>
        public static int RecCount = 0;
        /// <summary>
        /// 拒识短信总数
        /// </summary>
        public static int RefuseCount = 0;

        #region 应该识别而没有识别的短信
        /// <summary>
        /// 应该识别而没有识别的短信数目
        /// </summary>
        public static int SMSCount = 0;

        /// <summary>
        /// string代表预处理之后短信内容 int代表短信出现的次数
        /// </summary>
        public static Dictionary<string, int> SMSDic = new Dictionary<string, int>();
        /// <summary>
        /// 代表预处理之前的短信
        /// </summary>
        public static List<string> SMSList = new List<string>();
        #endregion

        #region 单字
        /// <summary>
        /// 单字的数目
        /// </summary>
        public static int DanziCount = 0;
        /// <summary>
        /// string代表预处理之后短信内容 int代表短信出现的次数
        /// </summary>
        public static Dictionary<string, int> DanziDic = new Dictionary<string, int>();
        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> DanziList = new List<string>();
        #endregion

        #region Empty短信
        /// <summary>
        /// 空短信的数目
        /// </summary>
        public static int EmptyCount = 0;

        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> EmptyList = new List<string>();

        #endregion

        #region 超长短信

        /// <summary>
        /// 超长短信数目
        /// </summary>
        public static int LongSMSCount = 0;

        /// <summary>
        /// 长短信内容，保存预处理之前的内容
        /// </summary>
        public static List<string> LongSMSList = new List<string>();
        #endregion

        #region 纯数字短信

        /// <summary>
        /// 纯数字的短信的数目
        /// </summary>
        public static int PureNumSMSCount = 0;

        /// <summary>
        /// string 预处理之后的短信 int短信的数目
        /// </summary>
        public static Dictionary<string, int> PureNumDic = new Dictionary<string, int>();

        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> PureNumList = new List<string>();
        #endregion

        #region 完全乱码
        /// <summary>
        /// 完全乱码的短信数目
        /// </summary>
        public static int FullGarbledCount = 0;

        /// <summary>
        /// string 预处理之后的的短信 int 短信的数目
        /// </summary>
        public static Dictionary<string, int> FullGarbledDic = new Dictionary<string, int>();

        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> FullGarbledList = new List<string>();
        #endregion

        #region 出现在拒识表中的短信
        /// <summary>
        /// 出现在拒识表中的短信的数目
        /// </summary>
        public static int SMSRefuseCount = 0;

        /// <summary>
        /// string 预处理之后的短信 int 短信的数目
        /// </summary>
        public static Dictionary<string, int> SMSRefuseDic = new Dictionary<string, int>();

        /// <summary>
        /// string 预处理之前的短信
        /// </summary>
        public static List<string> SMSRefuseList = new List<string>();
        #endregion

        #region 出现在talking.txt表中的短信
        /// <summary>
        /// 出现在talking.txt表中的短信
        /// </summary>
        public static int SMSTalkingCount = 0;

        /// <summary>
        /// string 预处理之后的短信 int 短信的数目
        /// </summary>
        public static Dictionary<string, int> SMSTalkingDic = new Dictionary<string, int>();

        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> SMSTalkingList = new List<string>();
        #endregion

        #region 作为单条处理的短信

        /// <summary>
        /// 其他类型的短信的数目
        /// </summary>
        public static int OtherCount = 0;

        /// <summary>
        /// string 预处理之后的短信 int 短信的数目
        /// </summary>
        public static Dictionary<string, int> OtherDic = new Dictionary<string, int>();

        /// <summary>
        /// 预处理之前的短信
        /// </summary>
        public static List<string> OtherList = new List<string>();

        #endregion

        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <returns></returns>
        public static bool ReadExcel()
        {
            Workbook workbook = null;
            Cells cells = null;

            try
            {
                workbook = new Workbook();
                workbook.Open(ConfigProxy.InputFile);
                cells = workbook.Worksheets[0].Cells;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            try
            {
                //3列 第1列 用户短信 第二列 业务名 第三 操作名
                for (int i = 1; i < cells.Rows.Count; i++)
                {
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine("正在处理第"+i+"数据");
                    }

                    InputItem item = new InputItem();
                    item.UserContent = cells.Rows[i][0].Value == null ? string.Empty : cells.Rows[i][0].Value.ToString().Trim();
                    item.BizName = cells.Rows[i][1].Value == null ? string.Empty : cells.Rows[i][1].Value.ToString().Trim();
                    item.OperaName = cells.Rows[i][2].Value == null ? string.Empty : cells.Rows[i][2].Value.ToString().Trim();

                    //短信总数加1
                    SumCount++;
                    if (item.IsRec)
                    {
                        //短信内容已经识别
                        RecCount++;
                        continue;
                    }
                    else
                    {
                        //未识别
                        RefuseCount++;
                    }
                    //保留预处理之前的短信
                    string temp = item.UserContent;
                    //短信内容预处理
                    item.UserContent = SMSPreProc.PreProc(item.UserContent);

                    #region 空短信
                    //空短信
                    if (string.IsNullOrEmpty(item.UserContent))
                    {
                        EmptyCount++;
                        EmptyList.Add(temp);
                        continue;
                    }
                    #endregion

                    #region 应该识别而没有识别的短信
                    //应该识别而没有识别
                    if (SMSProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSCount++;
                        //如果已经统计过，数目加1
                        if (SMSDic.ContainsKey(item.UserContent))
                        {
                            SMSDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSDic.Add(item.UserContent, 1);
                        }
                        SMSList.Add(temp);
                        continue;
                    }
                    #endregion

                    #region 长短信
                    int length = ConfigProxy.SMSMaxLen;

                    if (temp.Length > length)
                    {
                        LongSMSCount++;
                        LongSMSList.Add(temp);
                        continue;
                    }

                    #endregion

                    #region 单字短信
                    if (item.UserContent.Length <= 1)
                    {
                        DanziCount++;
                        //加入预处理之前的短信内容
                        DanziList.Add(temp);
                        if (DanziDic.ContainsKey(item.UserContent))
                        {
                            DanziDic[item.UserContent]++;
                        }
                        else
                        {
                            DanziDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 纯数字短信
                    if (TextCommonFunctions.IsNumber(item.UserContent))
                    {
                        PureNumSMSCount++;
                        PureNumList.Add(temp);
                        if (PureNumDic.ContainsKey(item.UserContent))
                        {
                            PureNumDic[item.UserContent]++;
                        }
                        else
                        {
                            PureNumDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 完全乱码
                    if (TextCommonFunctions.IsFullGarbled(item.UserContent))
                    {
                        FullGarbledCount++;
                        //保留预处理之前的短信
                        FullGarbledList.Add(temp);
                        if (FullGarbledDic.ContainsKey(item.UserContent))
                        {
                            FullGarbledDic[item.UserContent]++;
                        }
                        else
                        {
                            FullGarbledDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 出现在SMSRefuse.txt拒识表中的短信
                    if (SMSRefuseProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSRefuseCount++;
                        SMSRefuseList.Add(temp);
                        if (SMSRefuseDic.ContainsKey(item.UserContent))
                        {
                            SMSRefuseDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSRefuseDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 出现在Talking.txt中的短信
                    if (TalkingProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSTalkingCount++;
                        SMSTalkingList.Add(temp);
                        if (SMSTalkingDic.ContainsKey(item.UserContent))
                        {
                            SMSTalkingDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSTalkingDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 其它类型的短信，作为单条处理

                    OtherCount++;
                    OtherList.Add(temp);
                    if (OtherDic.ContainsKey(item.UserContent))
                    {
                        OtherDic[item.UserContent]++;
                    }
                    else
                    {
                        OtherDic.Add(item.UserContent, 1);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;

        }

        /// <summary>
        /// 从数据库中读取数据
        /// </summary>
        /// <returns></returns>
        public static bool ReadFromDb()
        {
            DataTable dt = null;
            try
            {
                dt = SqlHelper.GetInstance(ConfigProxy.ConnectionString).ExecuteDataTable(ConfigProxy.SelectSQL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取数据库失败"+ex.ToString());
                return false;
            }
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 100 == 0)
                    {
                        Console.WriteLine("正在处理第" + i + "数据");
                    }
                    InputItem item = new InputItem();
                    item.UserContent = dt.Rows[i][0] == null ? string.Empty : dt.Rows[i][0].ToString().Trim();
                    item.BizName = dt.Rows[i][1] == null ? string.Empty : dt.Rows[i][1].ToString().Trim();
                    item.OperaName = dt.Rows[i][2] == null ? string.Empty : dt.Rows[i][2].ToString().Trim();

                    //短信总数加1
                    SumCount++;
                    if (item.IsRec)
                    {
                        //短信内容已经识别
                        RecCount++;
                        continue;
                    }
                    else
                    {
                        //未识别
                        RefuseCount++;
                    }
                    //保留预处理之前的短信
                    string temp = item.UserContent;
                    //短信内容预处理
                    item.UserContent = SMSPreProc.PreProc(item.UserContent);

                    #region 空短信
                    //空短信
                    if (string.IsNullOrEmpty(item.UserContent))
                    {
                        EmptyCount++;
                        EmptyList.Add(temp);
                        continue;
                    }
                    #endregion

                    #region 应该识别而没有识别的短信
                    //应该识别而没有识别
                    if (SMSProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSCount++;
                        //如果已经统计过，数目加1
                        if (SMSDic.ContainsKey(item.UserContent))
                        {
                            SMSDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSDic.Add(item.UserContent, 1);
                        }
                        SMSList.Add(temp);
                        continue;
                    }
                    #endregion

                    #region 长短信
                    int length = ConfigProxy.SMSMaxLen;

                    if (temp.Length > length)
                    {
                        LongSMSCount++;
                        LongSMSList.Add(temp);
                        continue;
                    }

                    #endregion

                    #region 单字短信
                    if (item.UserContent.Length <= 1)
                    {
                        DanziCount++;
                        //加入预处理之前的短信内容
                        DanziList.Add(temp);
                        if (DanziDic.ContainsKey(item.UserContent))
                        {
                            DanziDic[item.UserContent]++;
                        }
                        else
                        {
                            DanziDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 纯数字短信
                    if (TextCommonFunctions.IsNumber(item.UserContent))
                    {
                        PureNumSMSCount++;
                        PureNumList.Add(temp);
                        if (PureNumDic.ContainsKey(item.UserContent))
                        {
                            PureNumDic[item.UserContent]++;
                        }
                        else
                        {
                            PureNumDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 完全乱码
                    if (TextCommonFunctions.IsFullGarbled(item.UserContent))
                    {
                        FullGarbledCount++;
                        //保留预处理之前的短信
                        FullGarbledList.Add(temp);
                        if (FullGarbledDic.ContainsKey(item.UserContent))
                        {
                            FullGarbledDic[item.UserContent]++;
                        }
                        else
                        {
                            FullGarbledDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 出现在SMSRefuse.txt拒识表中的短信
                    if (SMSRefuseProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSRefuseCount++;
                        SMSRefuseList.Add(temp);
                        if (SMSRefuseDic.ContainsKey(item.UserContent))
                        {
                            SMSRefuseDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSRefuseDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 出现在Talking.txt中的短信
                    if (TalkingProxy.DicItems.ContainsKey(item.UserContent))
                    {
                        SMSTalkingCount++;
                        SMSTalkingList.Add(temp);
                        if (SMSTalkingDic.ContainsKey(item.UserContent))
                        {
                            SMSTalkingDic[item.UserContent]++;
                        }
                        else
                        {
                            SMSTalkingDic.Add(item.UserContent, 1);
                        }
                        continue;
                    }
                    #endregion

                    #region 其它类型的短信，作为单条处理

                    OtherCount++;
                    OtherList.Add(temp);
                    if (OtherDic.ContainsKey(item.UserContent))
                    {
                        OtherDic[item.UserContent]++;
                    }
                    else
                    {
                        OtherDic.Add(item.UserContent, 1);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }


        /// <summary>
        /// 将结果保存到Excel中
        /// </summary>
        public static void SaveAsExcel()
        {
            string outputFile = "统计结果_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Workbook workbook = new Workbook();
            
            //workbook.Open(outputFile);
            #region 应该识别而没有识别的短信
            {
                workbook.Worksheets.Clear();
                workbook.Worksheets.Add();
                workbook.Worksheets[0].Name = "应该识别而没有识别的短信";
                workbook.Worksheets[0].Cells.Rows[0][0].PutValue("短信内容");
                workbook.Worksheets[0].Cells.Rows[0][1].PutValue("业务名");
                workbook.Worksheets[0].Cells.Rows[0][2].PutValue("操作名");
                workbook.Worksheets[0].Cells.Rows[0][3].PutValue("出现次数");
                int i = 1;
                foreach (string str in SMSDic.Keys)
                {
                    workbook.Worksheets[0].Cells.Rows[i][0].PutValue(str);
                    if (SMSProxy.DicItems.ContainsKey(str))
                    {
                        workbook.Worksheets[0].Cells.Rows[i][1].PutValue(SMSProxy.DicItems[str].BizName);
                        workbook.Worksheets[0].Cells.Rows[i][2].PutValue(SMSProxy.DicItems[str].OperaName);
                    }
                    workbook.Worksheets[0].Cells.Rows[i][3].PutValue(SMSDic[str]);
                    i++;
                }
               
            }
            #endregion

            #region 单字短信或者空短信
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[1].Name = "单字短信或者空短信";
                workbook.Worksheets[1].Cells.Rows[0][0].PutValue("短信内容");
                workbook.Worksheets[1].Cells.Rows[0][1].PutValue("出现次数");
                int i = 1;
                foreach (string str in EmptyList)
                {
                    workbook.Worksheets[1].Cells.Rows[i][0].PutValue(str);
                    workbook.Worksheets[1].Cells.Rows[i][1].PutValue("预处理之后为空");
                    i++;
                }
                foreach (string str in DanziDic.Keys)
                {
                    workbook.Worksheets[1].Cells.Rows[i][0].PutValue(str);
                    workbook.Worksheets[1].Cells.Rows[i][1].PutValue(DanziDic[str]);
                    i++;
                }
            }
            #endregion

            #region 超长短信
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[2].Name = "短信长度超过" + ConfigProxy.SMSMaxLen;
                workbook.Worksheets[2].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in LongSMSList)
                {
                    workbook.Worksheets[2].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 完全乱码
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[3].Name = "完全乱码";
                workbook.Worksheets[3].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in FullGarbledList)
                {
                    workbook.Worksheets[3].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 纯数字
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[4].Name = "纯数字";
                workbook.Worksheets[4].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in PureNumList)
                {
                    workbook.Worksheets[4].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 出现在拒识表中的短信
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[5].Name = "出现在拒识表中的短信";
                workbook.Worksheets[5].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in SMSRefuseList)
                {
                    workbook.Worksheets[5].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 出现在talking表中的短信
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[6].Name = "出现在talking表中的短信";
                workbook.Worksheets[6].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in SMSTalkingList)
                {
                    workbook.Worksheets[6].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 其它
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[7].Name = "其它";
                workbook.Worksheets[7].Cells.Rows[0][0].PutValue("短信内容");
                int i = 1;
                foreach (string str in OtherList)
                {
                    workbook.Worksheets[7].Cells.Rows[i][0].PutValue(str);
                    i++;
                }
            }
            #endregion

            #region 统计结果
            workbook.Worksheets.Add();
            workbook.Worksheets[8].Name = "统计结果";
            workbook.Worksheets[8].Cells.Rows[0][0].PutValue("拒识短信分类");
            workbook.Worksheets[8].Cells.Rows[0][1].PutValue("统计次数");
            workbook.Worksheets[8].Cells.Rows[0][2].PutValue("所占比例");

            if (RefuseCount > 0)
            {
                //应该识别而没有识别
                workbook.Worksheets[8].Cells.Rows[1][0].PutValue("应该识别而没有识别");
                workbook.Worksheets[8].Cells.Rows[1][1].PutValue(SMSCount);
                workbook.Worksheets[8].Cells.Rows[1][2].PutValue((double)SMSCount / RefuseCount);
                //空短信
                workbook.Worksheets[8].Cells.Rows[2][0].PutValue("空短信");
                workbook.Worksheets[8].Cells.Rows[2][1].PutValue(EmptyCount);
                workbook.Worksheets[8].Cells.Rows[2][2].PutValue((double)EmptyCount / RefuseCount);
                //单字短信
                workbook.Worksheets[8].Cells.Rows[3][0].PutValue("单字短信");
                workbook.Worksheets[8].Cells.Rows[3][1].PutValue(DanziCount);
                workbook.Worksheets[8].Cells.Rows[3][2].PutValue((double)DanziCount / RefuseCount);
                //超长短信
                workbook.Worksheets[8].Cells.Rows[4][0].PutValue("短信长度超过" + ConfigProxy.SMSMaxLen);
                workbook.Worksheets[8].Cells.Rows[4][1].PutValue(LongSMSCount);
                workbook.Worksheets[8].Cells.Rows[4][2].PutValue((double)LongSMSCount / RefuseCount);
                //完全乱码
                workbook.Worksheets[8].Cells.Rows[5][0].PutValue("完全乱码");
                workbook.Worksheets[8].Cells.Rows[5][1].PutValue(FullGarbledCount);
                workbook.Worksheets[8].Cells.Rows[5][2].PutValue((double)FullGarbledCount / RefuseCount);
                //纯数字
                workbook.Worksheets[8].Cells.Rows[6][0].PutValue("纯数字");
                workbook.Worksheets[8].Cells.Rows[6][1].PutValue(PureNumSMSCount);
                workbook.Worksheets[8].Cells.Rows[6][2].PutValue((double)PureNumSMSCount / RefuseCount);
                //出现在拒识表中的短信
                workbook.Worksheets[8].Cells.Rows[7][0].PutValue("出现在拒识表中的短信");
                workbook.Worksheets[8].Cells.Rows[7][1].PutValue(SMSRefuseCount);
                workbook.Worksheets[8].Cells.Rows[7][2].PutValue((double)SMSRefuseCount / RefuseCount);
                //出现在talking表中的短信
                workbook.Worksheets[8].Cells.Rows[8][0].PutValue("出现在talking表中的短信");
                workbook.Worksheets[8].Cells.Rows[8][1].PutValue(SMSTalkingCount);
                workbook.Worksheets[8].Cells.Rows[8][2].PutValue((double)SMSTalkingCount / RefuseCount);
                //其它
                workbook.Worksheets[8].Cells.Rows[9][0].PutValue("其它");
                workbook.Worksheets[8].Cells.Rows[9][1].PutValue(OtherCount);
                workbook.Worksheets[8].Cells.Rows[9][2].PutValue((double)OtherCount / RefuseCount);
                //合计
                workbook.Worksheets[8].Cells.Rows[10][0].PutValue("合计");
                workbook.Worksheets[8].Cells.Rows[10][1].PutValue(RefuseCount);
                workbook.Worksheets[8].Cells.Rows[10][2].PutValue((double)RefuseCount / RefuseCount);
            }

            #endregion
            workbook.Save(outputFile);
        }

        /// <summary>
        /// 将结果保存在数据库中
        /// </summary>
        public static void SaveAsTxt()
        {
            string outputFile = "统计结果_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (!Directory.Exists(outputFile))
            {
                Directory.CreateDirectory(outputFile);
            }

            #region 应该识别而没有识别的短信
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("短信内容\t业务名\t操作名\t出现次数");
                foreach (string str in SMSDic.Keys)
                {
                    sb.Append(str + "\t");
                    if (SMSProxy.DicItems.ContainsKey(str))
                    {
                        sb.Append(SMSProxy.DicItems[str].BizName + "\t");
                        sb.Append(SMSProxy.DicItems[str].OperaName + "\t");
                    }
                    sb.Append(SMSDic[str] + "\r\n");
                }
                string filename = ".\\" + outputFile + "\\" + "应该识别但没有识别的短信.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 单字短信或者空短信
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("空短信");
                foreach (string str in EmptyList)
                {
                    if (str != null)
                    {
                        sb.AppendLine(str);
                    }
                }
                string filename = ".\\" + outputFile + "\\" + "空短信.txt";
                FileHelper.WriteFile(sb.ToString(), filename);

                StringBuilder sb2 = new StringBuilder();
                sb2.AppendLine("单字");
                foreach (string str in DanziList)
                {
                    sb2.AppendLine(str);
                }
                string filename2 = ".\\" + outputFile + "\\" + "单字.txt";
                FileHelper.WriteFile(sb.ToString(), filename2);
            }
            #endregion

            #region 超长短信
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("短信长度超过" + ConfigProxy.SMSMaxLen);
                foreach (string str in LongSMSList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "短信超长.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 完全乱码
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("完全乱码");
                foreach (string str in FullGarbledList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "完全乱码.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 纯数字
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("纯数字");
                foreach (string str in PureNumList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "纯数字.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 出现在拒识表中的短信
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("出现在拒识表中的短信");
                foreach (string str in SMSRefuseList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "出现在拒识表中的短信.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 出现在talking表中的短信
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("出现在talking表中的短信");
                foreach (string str in SMSTalkingList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "出现在talking表中的短信.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 其它
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("其它");
                foreach (string str in OtherList)
                {
                    sb.AppendLine(str);
                }
                string filename = ".\\" + outputFile + "\\" + "其它.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion

            #region 统计结果
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("拒识短信分类\t统计次数\t所占比例");
                if (RefuseCount > 0)
                {
                    sb.AppendLine("应该识别而没有识别\t" + SMSCount + "\t" + ((double)SMSCount / RefuseCount));
                    sb.AppendLine("空短信\t" + EmptyCount + "\t" + ((double)EmptyCount / RefuseCount));
                    sb.AppendLine("单字短信\t" + DanziCount + "\t" + ((double)DanziCount / RefuseCount));
                    sb.AppendLine("短信长度超过" + ConfigProxy.SMSMaxLen + "\t" + LongSMSCount + "\t" + ((double)LongSMSCount / RefuseCount));
                    sb.AppendLine("完全乱码\t" + FullGarbledCount + "\t" + ((double)FullGarbledCount / RefuseCount));
                    sb.AppendLine("纯数字\t" + PureNumSMSCount + "\t" + ((double)PureNumSMSCount / RefuseCount));
                    sb.AppendLine("出现在拒识表中的短信\t" + SMSRefuseCount + "\t" + ((double)SMSRefuseCount / RefuseCount));
                    sb.AppendLine("出现在talking表中的短信\t" + SMSTalkingCount + "\t" + ((double)SMSTalkingCount / RefuseCount));
                    sb.AppendLine("其它\t" + OtherCount + "\t" + ((double)OtherCount / RefuseCount));
                    sb.AppendLine("合计\t" + RefuseCount + "\t" + ((double)RefuseCount / RefuseCount));
                }
                string filename = ".\\" + outputFile + "\\" + "统计结果.txt";
                FileHelper.WriteFile(sb.ToString(), filename);
            }
            #endregion
        }

    }


    /// <summary>
    /// 一个输入项
    /// </summary>
    public class InputItem
    {
        /// <summary>
        /// 用户短信内容
        /// </summary>
        public string UserContent;

        /// <summary>
        /// 业务名
        /// </summary>
        public string BizName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InputItem()
        {
            UserContent = string.Empty;
            BizName = string.Empty;
            OperaName = string.Empty;
        }

        /// <summary>
        /// 操作名
        /// </summary>
        public string OperaName;

        /// <summary>
        /// 数据项是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(UserContent);
            }
        }

        /// <summary>
        /// 判断短信内容是否识别
        /// </summary>
        public bool IsRec
        {
            get
            {
                return BizName == null ? false : !string.IsNullOrEmpty(BizName.Trim());
            }
        }

    }


}
