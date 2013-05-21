using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

namespace iFlyTek.ECSS30.WirelessCity
{
    public class BasePage:System.Web.UI.Page
    {
        #region 覆盖基类的方法

        /// <summary>
        /// 主要处理每次请求都要做的工作
        /// 仍然将Page_Load方法留给子类去实现。（目前未在子类中实现Page_Load）
        /// 它主要实现公用的代码，如每个页面都要进行的判断（夹寄卡的入口）。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //获取查询字符串值
            Caller = GetQueryString("caller");
            Callid = GetQueryString("callid");
            Called = GetQueryString("called");
            OriCaller = GetQueryString("oricaller");
        }

        #endregion

        #region 一些公共字段

        /// <summary>
        /// 主叫号码
        /// </summary>
        public string Caller;

        /// <summary>
        /// 会话id
        /// </summary>
        public string Callid;

        /// <summary>
        /// 被叫号码
        /// </summary>
        public string Called;

        /// <summary>
        /// 原始主叫号码，因为在转接的情况下，caller代表的是转接的号码
        /// </summary>
        public string OriCaller;
        #endregion

        #region Session相关操作

        /// <summary>
        /// 获取Session变量，并将其转换为字符串。如果获取不到，返回string.Empty。
        /// </summary>
        /// <param name="key">会话值的键名</param>
        /// <returns>返回会话值。如果获取不到会话值，返回string.Empty</returns>
        public string GetSessionValueOfString(string key)
        {
            if (key == null || string.IsNullOrEmpty(key.Trim()))
            {
                return string.Empty;
            }

            if (Session[key] == null)
            {
                return string.Empty;
            }

            return Session[key].ToString();
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">会话值的键名</param>
        /// <returns>返回会话值。如果获取不到会话值，返回null</returns>
        public object GetSessionValueOfObject(string key)
        {
            if (key == null || string.IsNullOrEmpty(key.Trim()))
            {
                return null;
            }

            if (Session[key] == null)
            {
                return null;
            }

            return Session[key];
        }

        /// <summary>
        /// 将一个值放入到Session中,如果 key 参数引用现有的会话状态项，则当前项将使用指定 value 覆盖。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void PutSessionValue(string key, object value)
        {
            //key不能为null或空串，value不能为null
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                //如果 name 参数引用现有的会话状态项，则当前项将使用指定 value 改写。
                Session.Add(key, value);
            }
        }

        /// <summary>
        /// 清空Session中的所有值
        /// </summary>
        public void ClearSessionValue()
        {
            Session.Clear();
        }

        /// <summary>
        /// 删除指定key的Session值
        /// 如果会话状态集合不包含带有指定 key 的元素，则该集合保持不变。不引发异常。
        /// </summary>
        /// <param name="key"></param>
        public void DeleteSessionValue(string key)
        {
            //如果会话状态集合不包含带有指定 name 的元素，则该集合保持不变。不引发异常。
            if (!string.IsNullOrEmpty(key))
            {
                Session.Remove(key);
            }
        }

        #endregion

        #region 处理vxml相关操作

        /// <summary>
        /// 回发vxml内容,为了提高效率，传递引用
        /// </summary>
        /// <param name="strVxml">需要回发的vxml，为了提高效率，传递引用。</param>
        public void WriteResponseVxml(ref string strVxml)
        {
            byte[] streamBuff = System.Text.ASCIIEncoding.Default.GetBytes(strVxml);
            Response.BinaryWrite(streamBuff);
        }

        /// <summary>
        /// 用于读取vxml文件，如<code>ReadVxmlFile(Server.MapPath("../vxmls/"), "ActivateCard_Common.xml");</code>，发生异常，返回空串
        /// </summary>
        /// <param name="path">文件所在的目录名，它与文件名拼接成实际的文件的路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>读取到文件的内容。</returns>
        public string ReadVxmlFile(string path, string fileName)
        {
            try
            {
                string strTemp = string.Empty;
                string filePath = path + fileName;
                using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
                {
                    strTemp = reader.ReadToEnd();
                    return strTemp;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 用于读取vxml文件
        /// </summary>
        /// <param name="filePath">文件的路径名</param>
        /// <returns>读取到文件的内容</returns>
        public string ReadVxmlFile(string filePath)
        {
            try
            {
                string strTemp = string.Empty;
                using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
                {
                    strTemp = reader.ReadToEnd();
                }
                return strTemp;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 译码 （将xml中的以&开头以；结束的部分替换为实际的符号）
        /// </summary>
        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            string output = input;
            output = System.Text.RegularExpressions.Regex.Replace(output, "&amp;", "&");
            output = System.Text.RegularExpressions.Regex.Replace(output, "&lt;", "<");
            output = System.Text.RegularExpressions.Regex.Replace(output, "&gt;", ">");
            output = System.Text.RegularExpressions.Regex.Replace(output, "&quot;", "\"");
            output = System.Text.RegularExpressions.Regex.Replace(output, "&apos;", "\'");
            return output;
        }

        /// <summary>
        /// 编码  （将xml中的实际的符号替换为以&开头以；结束的内容）
        /// </summary>
        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty; ;
            }

            string output = input;
            output = System.Text.RegularExpressions.Regex.Replace(output, "&", "&amp;");
            output = System.Text.RegularExpressions.Regex.Replace(output, "<", "&lt;");
            output = System.Text.RegularExpressions.Regex.Replace(output, ">", "&gt;");
            output = System.Text.RegularExpressions.Regex.Replace(output, "\"", "&quot;");
            output = System.Text.RegularExpressions.Regex.Replace(output, "\'", "&apos;");
            return output;
        }

        #endregion

        #region 查询字符串相关

        /// <summary>
        /// 获取查询字符串,如果找不到返回string.Empty
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paramName">要获取集合成员的key</param>
        /// <returns>如果找不到返回string.Empty</returns>
        public string GetQueryString(string key)
        {
            //if (string.IsNullOrEmpty(Request[key]))
            //{
            //    return string.Empty;
            //}
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            //return Request[key];
            return Request[key]??string.Empty;
        }

        #endregion
    }
}
