using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iFlyTek.ECSS30.Tool.SMSProc;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 正常识别的短信代理
    /// </summary>
    public class SMSProxy
    {
        /// <summary>
        /// 正常识别短信内容
        /// </summary>
        public static Dictionary<string, SMSItem> DicItems = new Dictionary<string, SMSItem>();

        /// <summary>
        /// 加载正常识别短信内容
        /// </summary>
        /// <returns></returns>
        public static bool LoadSMS()
        {
            bool result = false;

            DicItems.Clear();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(ConfigProxy.SMSFile,Encoding.Default);
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    string[] temp = str.Split('\t');
                    SMSItem item = new SMSItem();
                    switch (temp.Length)
                    {
                        case 1:
                            item.UserContent = temp[0];
                            break;
                        case 2:
                            item.UserContent = temp[0];
                            item.BizName = temp[1];
                            break;
                        case 3:
                            item.UserContent = temp[0];
                            item.BizName = temp[1];
                            item.OperaName = temp[2];
                            break;

                    }
                    //短信预处理
                    item.UserContent = SMSPreProc.PreProc(item.UserContent);
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
    /// 正常识别的短信项
    /// </summary>
    public class SMSItem
    {
        /// <summary>
        /// 短信内容
        /// </summary>
        public string UserContent;

        /// <summary>
        /// 业务名
        /// </summary>
        public string BizName;

        /// <summary>
        /// 操作名
        /// </summary>
        public string OperaName;

        public SMSItem()
        {
            UserContent = string.Empty;
            BizName = string.Empty;
            OperaName = string.Empty;
        }

        /// <summary>
        /// 短信内容不为空即为有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(UserContent);
            }
        }
    }
}
