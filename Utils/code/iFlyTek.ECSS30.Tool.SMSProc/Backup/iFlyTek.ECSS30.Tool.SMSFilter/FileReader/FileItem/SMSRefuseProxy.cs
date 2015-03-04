using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iFlyTek.ECSS30.Tool.SMSProc;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 拒识短信表
    /// </summary>
    public class SMSRefuseProxy
    {
        /// <summary>
        /// 存放拒绝短信数据
        /// </summary>
        public static Dictionary<string, SMSRefuseItem> DicItems = new Dictionary<string, SMSRefuseItem>();

        /// <summary>
        /// 加载拒绝短信数据
        /// </summary>
        public static bool LoadSMSRefuse()
        {
            bool result = false;
            DicItems.Clear();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(ConfigProxy.SmsRefuse, Encoding.Default);
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    SMSRefuseItem item = new SMSRefuseItem();
                    //短信预处理
                    item.UserContent = SMSPreProc.PreProc(str);
                    if (item.IsValid && !DicItems.ContainsKey(item.UserContent))
                    {
                        DicItems.Add(item.UserContent, item);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return result;
        }
    }

    /// <summary>
    /// 拒识短信项
    /// </summary>
    public class SMSRefuseItem
    {
        /// <summary>
        /// 短信内容
        /// </summary>
        public string UserContent;

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(UserContent);
            }
        }
    }
}
