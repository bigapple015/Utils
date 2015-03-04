using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iFlyTek.ECSS30.Tool.SMSProc;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// talking文件代理
    /// </summary>
    public class TalkingProxy
    {
        /// <summary>
        /// 存放talking短信
        /// </summary>
        public static Dictionary<string,TalkingItem> DicItems = new Dictionary<string,TalkingItem>();

        /// <summary>
        /// 加载talking
        /// </summary>
        /// <returns></returns>
        public static bool LoadTalking()
        {
            bool result = false;
            DicItems.Clear();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(ConfigProxy.Talking, Encoding.Default);
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    TalkingItem item = new TalkingItem();
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

        /// <summary>
        /// 一项talking数据
        /// </summary>
        public class TalkingItem
        {
            /// <summary>
            /// 短信内容
            /// </summary>
            public string UserContent;

            /// <summary>
            /// 判断是否有效
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
}
