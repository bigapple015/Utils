using System;   
using System.Collections.Generic;   
using System.Linq;   
using System.Text;   
using System.Text.RegularExpressions;   
  
namespace Common   
{   
    /// <summary>   
    /// 字符串操作类   
    /// </summary>   
    public class StringHelper   
    {   
        public StringHelper()   
        {   
  
        }   
  
        /// <summary>
        /// 无效的字符，主要是取出源字符串中的\r\n，防止破坏请求包
        /// </summary>
        private const string InvalidPunction = "\r\n\t ";

        /// <summary>
        /// 将Null转换为Empty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string NullToEmptyAndTrim(string source)
        {
            if(source == null)
            {
                return string.Empty;
            }
            source = source.Trim();
            if(source == string.Empty)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            foreach (char ch in source)
            {
                //去除无效字符，主要是\r\n，防止破坏请求包的格式
                if(InvalidPunction.IndexOf(ch) >= 0)
                {
                    continue;
                }
                else
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }

  
        /// <summary>   
        /// 出错时弹出提示对话框   
        /// </summary>   
        /// <param name="str_Control_Name">检验控件id值</param>   
        /// <param name="str_Form_Name">表单id值</param>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string JsIsNull(string str_Control_Name, string str_Form_Name, string str_Prompt)   
        {   
            return  "<script language=\"javascript\">alert('" + str_Prompt + "');document." + str_Form_Name + "." + str_Control_Name + ".focus(); document." + str_Form_Name + "." + str_Control_Name + ".select();</" + "script>";   
        }   
  
  
        /// <summary>   
        /// 出错时弹出提示对话框   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string JsIsNull(string str_Prompt)   
        {   
            return  "<script language=\"javascript\">alert('" + str_Prompt + "');</" + "script>";   
        }   
  
  
        /// <summary>   
        /// 关闭对话框   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string  CloseParent(string str_Prompt)   
        {   
            return  "<script language=\"javascript\">alert('" + str_Prompt + "');window.parent.close();</" + "script>";   
        }   
  
        /// <summary>   
        /// 出错时弹出提示对话框--关闭窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="isReLoad">true为上个窗口自动刷新</param>   
        /// <returns>string</returns>   
        public static string JsIsNull(string str_Prompt, bool isReLoad)   
        {   
            if (isReLoad)   
            {   
                return  "<script language=\"javascript\">alert('" + str_Prompt + "');opener.window.document.location.reload();window.close();</" + "script>";   
            }   
            else  
            {   
                return  "<script language=\"javascript\">alert('" + str_Prompt + "');window.close();</" + "script>";   
            }   
        }   
  
        /// <summary>   
        ///是否关闭窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="isClose">true为关闭</param>   
        /// <returns>string</returns>   
        public static string  JsIsClose(string str_Prompt,  bool isClose)   
        {   
            if (!isClose)   
            {   
                return  "<script language=\"javascript\">alert('" + str_Prompt + "');</" + "script>";   
            }   
            else  
            {   
                return  "<script language=\"javascript\">alert('" + str_Prompt + "');window.close();opener.window.document.location.reload();</" + "script>";   
            }   
        }   
  
        /// <summary>   
        /// 弹出信息并重装窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="reLoadPath">重装路径</param>   
        /// <returns>string</returns>   
        public static string  JsIsReLoad(string str_Prompt, string reLoadPath)   
        {   
            return  "<script language=\"javascript\">alert('" + str_Prompt + "');this.window.document.location.reload('" + reLoadPath + "');</" + "script>";   
        }   
  
        /// <summary>   
        /// 重装窗口   
        /// </summary>   
        /// <param name="reLoadPath">提示信息</param>   
        /// <returns>string</returns>   
        public static string  JsIsReLoad(string reLoadPath)   
        {   
            return  "<script language=\"javascript\">this.window.document.location.reload('" + reLoadPath + "');</" + "script>";   
        }   
  
        /// <summary>   
        /// 获得一个16位时间随机数   
        /// </summary>   
        /// <returns>返回随机数</returns>   
        public static string GetDataRandom()   
        {   
            string strData = DateTime.Now.ToString();   
            strData = strData.Replace(":", "");   
            strData = strData.Replace("-", "");   
            strData = strData.Replace(" ", "");   
            Random r = new Random();   
            strData = strData + r.Next(100000);   
            return strData;   
        }   
  
        /// <summary>   
        ///  获得某个字符串在另个字符串中出现的次数   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static int GetStrCount(string strOriginal, string strSymbol)   
        {   
            int count = 0;   
            for (int i = 0; i < (strOriginal.Length - strSymbol.Length + 1); i++)   
            {   
                if (strOriginal.Substring(i, strSymbol.Length) == strSymbol)   
                {   
                    count = count + 1;   
                }   
            }   
            return count;   
        }   
  
        /// <summary>   
        /// 获得某个字符串在另个字符串第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetFirstStr(string strOriginal, string strSymbol)   
        {   
            int strPlace = strOriginal.IndexOf(strSymbol);   
            if (strPlace != -1)   
                strOriginal = strOriginal.Substring(0, strPlace);   
            return strOriginal;   
        }   
  
        /// <summary>   
        /// 获得某个字符串在另个字符串最后一次出现时后面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetLastStr(string strOriginal, string strSymbol)   
        {   
            int strPlace = strOriginal.LastIndexOf(strSymbol) + strSymbol.Length;   
            strOriginal = strOriginal.Substring(strPlace);   
            return strOriginal;   
        }   
  
        /// <summary>   
        /// 获得两个字符之间第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleFirstStr(string strOriginal, string strFirst, string strLast)   
        {   
            strOriginal = GetFirstStr(strOriginal, strLast);   
            strOriginal = GetLastStr(strOriginal, strFirst);   
            return strOriginal;   
        }   
  
        /// <summary>   
        ///  获得两个字符之间最后一次出现时的所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleLastStr(string strOriginal, string strFirst, string strLast)   
        {   
            strOriginal = GetLastStr(strOriginal, strFirst);   
            strOriginal = GetFirstStr(strOriginal, strLast);   
            return strOriginal;   
        }   
  
        /// <summary>   
        /// 从数据库表读记录时,能正常显示   
        /// </summary>   
        /// <param name="strContent">要处理的字符</param>   
        /// <returns>返回正常值</returns>   
        public static string GetHtmlFormat(string strContent)   
        {   
            strContent = strContent.Trim();   
  
            if (strContent == null)   
            {   
                return "";   
            }   
            strContent = strContent.Replace("<", "<");   
            strContent = strContent.Replace(">", ">");   
            strContent = strContent.Replace("\n", "<br />");   
            return (strContent);   
        }   
  
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">字符串1</param>   
        /// <param name="checkStr">字符串2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(string str, string checkStr, string reStr)   
        {   
            if (str == checkStr)   
            {   
                return reStr;   
            }   
            return "";   
        }   
  
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">数值1</param>   
        /// <param name="checkStr">数值2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(int str, int checkStr, string reStr)   
        {   
            if (str == checkStr)   
            {   
                return reStr;   
            }   
            return "";   
        }   
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(bool str, bool checkStr, string reStr)   
        {   
            if (str == checkStr)   
            {   
                return reStr;   
            }   
            return "";   
        }   
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(object str, object checkStr, string reStr)   
        {   
            if (str == checkStr)   
            {   
                return reStr;   
            }   
            return "";   
        }   
        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用endStr结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="endStr">超过字数，结束字符串，如"..."</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length, string endStr)   
        {   
            string reStr;   
            if (length < GetStrLength(str))   
            {   
                reStr = str.Substring(0, length) + endStr;   
            }   
            else  
            {   
                reStr = str;   
            }   
            return reStr;   
        }   
  
        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length)   
        {   
            string reStr;   
            if (length < str.Length)   
            {   
                reStr = str.Substring(0, length) + "...";   
            }   
            else  
            {   
                reStr = str;   
            }   
            return reStr;   
        }   
  
        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="subcount">若超过字数右边减少的字符长度</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length,int subcount)   
        {   
            string reStr;   
            if (length < str.Length)   
            {   
                reStr = str.Substring(0, length-subcount) + "...";   
            }   
            else  
            {   
                reStr = str;   
            }   
            return reStr;   
        }   
  
        /// <summary>   
        /// 获得双字节字符串的字节数   
        /// </summary>   
        /// <param name="str">要检测的字符串</param>   
        /// <returns>返回字节数</returns>   
        public static int GetStrLength(string str)   
        {   
            ASCIIEncoding n = new ASCIIEncoding();   
            byte[] b = n.GetBytes(str);   
            int l = 0;  // l 为字符串之实际长度   
            for (int i = 0; i < b.Length; i++)   
            {   
                if (b[i] == 63)  //判断是否为汉字或全脚符号   
                {   
                    l++;   
                }   
                l++;   
            }   
            return l;   
        }   
  
        /// <summary>   
        /// 剥去HTML标签   
        /// </summary>   
        /// <param name="text">带有HTML格式的字符串</param>   
        /// <returns>string</returns>   
        public static string RegStripHtml(string text)   
        {   
            string reStr;   
            string RePattern = @"<\s*(\S+)(\s[^>]*)?>";   
            reStr = Regex.Replace(text, RePattern, string.Empty, RegexOptions.Compiled);   
            reStr = Regex.Replace(reStr, @"\s+", string.Empty, RegexOptions.Compiled);   
            return reStr;   
        }   
  
        /// <summary>   
        /// 使Html失效,以文本显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <returns>失效后字符串</returns>   
        public static string ReplaceHtml(string str)   
        {   
            str = str.Replace("<", "<");   
            return str;   
        }   
  
  
        /// <summary>   
        /// 获得随机数字   
        /// </summary>   
        /// <param name="Length">随机数字的长度</param>   
        /// <returns>返回长度为 Length 的　<see cref="System.Int32"/> 类型的随机数</returns>   
        /// <example>   
        /// Length 不能大于9,以下为示例演示了如何调用 GetRandomNext：<br />   
        /// <code>   
        ///  int le = GetRandomNext(8);   
        /// </code>   
        /// </example>   
        public static int GetRandomNext(int Length)   
        {   
            if (Length > 9)   
                throw new System.IndexOutOfRangeException("Length的长度不能大于10");   
            Guid gu = Guid.NewGuid();   
            string str = "";   
            for (int i = 0; i < gu.ToString().Length; i++)   
            {   
                if (isNumber(gu.ToString()[i]))   
                {   
                    str += ((gu.ToString()[i]));   
                }   
            }   
            int guid = int.Parse(str.Replace("-", "").Substring(0, Length));   
            if (!guid.ToString().Length.Equals(Length))   
                guid = GetRandomNext(Length);   
            return guid;   
        }   
  
        /// <summary>   
        /// 返回一个 bool 值，指明提供的值是不是整数   
        /// </summary>   
        /// <param name="obj">要判断的值</param>   
        /// <returns>true[是整数]false[不是整数]</returns>   
        /// <remarks>   
        ///  isNumber　只能判断正(负)整数，如果 obj 为小数则返回 false;   
        /// </remarks>   
        /// <example>   
        /// 下面的示例演示了判断 obj 是不是整数：<br />   
        /// <code>   
        ///  bool flag;   
        ///  flag = isNumber("200");   
        /// </code>   
        /// </example>   
        public static bool isNumber(object obj)   
        {   
            //为指定的正则表达式初始化并编译 Regex 类的实例   
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^-?(\d*)$");   
            //在指定的输入字符串中搜索 Regex 构造函数中指定的正则表达式匹配项   
            System.Text.RegularExpressions.Match mc = rg.Match(obj.ToString());   
            //指示匹配是否成功   
            return (mc.Success);   
        }   
  
        /// <summary>   
        /// 高亮显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <param name="findstr">查找字符串</param>   
        /// <param name="cssclass">Style</param>   
        /// <returns>string</returns>   
        public static string OutHighlightText(string str, string findstr, string cssclass)   
        {   
            if (findstr != "")   
            {   
                string text1 = "<span class=\"" + cssclass + "\">%s</span>";   
                str = str.Replace(findstr, text1.Replace("%s", findstr));   
            }   
            return str;   
        }   
  
        /// <summary>   
        /// 移除字符串首尾某些字符   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startStr">要在字符串首部移除的字符串</param>   
        /// <param name="endStr">要在字符串尾部移除的字符串</param>   
        /// <returns>string</returns>   
        public static string RemoveStartOrEndStr(string strOriginal, string startStr,string endStr)   
        {   
            char[] start=startStr.ToCharArray();   
            char[] end=endStr.ToCharArray();   
            return strOriginal.TrimStart(start).TrimEnd(end);   
        }   
  
        /// <summary>   
        /// 删除指定位置指定长度字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startIndex">开始删除字符的位置</param>   
        /// <param name="count">要删除的字符数</param>   
        /// <returns>string</returns>   
        public static string RemoveStr(string strOriginal,int startIndex, int count)   
        {   
            return strOriginal.Remove(startIndex, count);   
        }   
  
        /// <summary>   
        /// 从左边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string LeftPadStr(string strOriginal, int totalWidth, char paddingChar)   
        {   
            if(strOriginal.Length<totalWidth)   
                return strOriginal.PadLeft(totalWidth, paddingChar);   
            return strOriginal;   
        }   
  
        /// <summary>   
        /// 从右边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string RightPadStr(string strOriginal, int totalWidth, char paddingChar)   
        {   
            if (strOriginal.Length < totalWidth)   
                return strOriginal.PadRight(totalWidth, paddingChar);   
            return strOriginal;   
        }   
    }   
}  
