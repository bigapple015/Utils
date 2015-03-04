using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 配置代理
    /// </summary>
    public class ConfigProxy
    {
        /// <summary>
        /// 短信最大长度
        /// </summary>
        public static int SMSMaxLen;

        /// <summary>
        /// 正常短信文件
        /// </summary>
        public static string SMSFile;

        /// <summary>
        /// 存放正常汉字文件
        /// </summary>
        public static string HanZi;

        /// <summary>
        /// talking文件
        /// </summary>
        public static string Talking;

        /// <summary>
        /// 繁简体表
        /// </summary>
        public static string Trad2Simp;

        /// <summary>
        /// 拒识短信列表
        /// </summary>
        public static string SmsRefuse;

        /// <summary>
        /// 数据的来源 
        /// 0 表示来自Excel
        /// 1 表示来自数据库
        /// </summary>
        public static string DataSource;

        /// <summary>
        /// 数据的输出
        /// 0 表示输出Excel
        /// 1 表示输出txt
        /// </summary>
        public static string DataOutput;

        /// <summary>
        /// 输入文件
        /// </summary>
        public static string InputFile;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString;

        /// <summary>
        /// SELECT语句
        /// </summary>
        public static string SelectSQL;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ConfigProxy()
        {
            //获取短信最大长度
            if (!int.TryParse(ConfigurationManager.AppSettings["SMSMaxLen"], out SMSMaxLen))
            {
                SMSMaxLen = 1000000;
            }

            SMSFile = ConfigurationManager.AppSettings["sms"] ?? string.Empty;
            HanZi = ConfigurationManager.AppSettings["hanzi"] ?? string.Empty;
            Talking = ConfigurationManager.AppSettings["talking"] ?? string.Empty;
            Trad2Simp = ConfigurationManager.AppSettings["trad2Simp"] ?? string.Empty;
            SmsRefuse = ConfigurationManager.AppSettings["smsrefuse"] ?? string.Empty;
            DataSource = ConfigurationManager.AppSettings["DataSource"] ?? string.Empty;
            InputFile = ConfigurationManager.AppSettings["InputFile"]??string.Empty;
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]??string.Empty;
            SelectSQL = ConfigurationManager.AppSettings["SelectSQL"]??string.Empty;
            DataOutput = ConfigurationManager.AppSettings["DataOutput"] ?? string.Empty;
        }

        /// <summary>
        /// 判断是否有效
        /// </summary>
        public static bool IsValid
        {
            get
            {
                return SMSMaxLen > 0 &&
                    File.Exists(SMSFile) && File.Exists(HanZi) &&
                    File.Exists(Talking) && File.Exists(Trad2Simp) &&
                    File.Exists(SmsRefuse)&&File.Exists(InputFile)&&
                    !string.IsNullOrEmpty(ConnectionString)&&
                    !string.IsNullOrEmpty(SelectSQL)&&!string.IsNullOrEmpty(DataSource);
            }
        }

        /// <summary>
        /// 判断数据输入是否来自Excel
        /// </summary>
        /// <returns></returns>
        public static bool IsInputFromExcel()
        {
            switch (DataSource)
            {
                case "0":
                    return true;
                case "1":
                    return false;
                default:
                    throw new Exception("DataSource配置项出错");
            }
        }


        public static bool IsOutPutToExcel()
        {
            switch (DataOutput)
            {
                case "0":
                    return true;
                case "1":
                    return false;
                default:
                    throw new Exception("DataOutput配置项出错");
            }
        }

    }
}
