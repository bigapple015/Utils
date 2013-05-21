using System;
using System.Collections.Generic;
using System.Text;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 一系列晓得文本方面的函数
    /// </summary>
    public class TextCommonFunctions
    {
        /// <summary>
        /// 判断输入字符串是否完全是数字
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>空串和null 返回true，完全是数字返回true</returns>
        public static bool IsNumber(string strInput)
        {
            foreach (char ch in strInput)
            {
                if (ch < '0' || ch > '9')
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断输入字符串是否完全是英文(A-Z a-z)
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>空串和null返回true，完全是英文字符返回true</returns>
        public static bool IsEnglish(string strInput)
        {
            foreach (char ch in strInput)
            {
                if ('A' > ch || ('Z' < ch && 'a' > ch) || ch > 'z')
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断输入字符串是否完全是数字或者英文组成
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>空串和null 返回true</returns>
        public static bool IsNumOrEng(string strInput)
        {
            foreach (char ch in strInput)
            {
                //不是数字也不是字母
                if (('A' > ch || ('Z' < ch && 'a' > ch) || ch > 'z')/*不为字母*/ &&
                    (ch < '0' || ch > '9')/*不为数字*/
                    )
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 是否包含中文字符
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>null和空串 返回false</returns>
        public static bool IsContainChinese(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
            {
                return false;
            }

            string strRex = @"[\u4e00-\u9fa5]";
            return System.Text.RegularExpressions.Regex.IsMatch(strInput, strRex);
        }

        /// <summary>
        /// 判断是否完全是中文
        /// </summary>
        /// <param name="strInput">输入短信</param>
        /// <returns>null和空串返回true；完全是中文返回true</returns>
        public static bool IsChinese(string strInput)
        {
            if (HanziProxy.DicItems != null)
            {
                foreach (char ch in strInput)
                {
                    if (!HanziProxy.DicItems.ContainsKey(ch))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 去除空白之后，是否只有0或1个字符
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool IsTrimToOneOrZero(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
            {
                return true;
            }

            return strInput.Trim().Length <= 1;
        }

        /// <summary>
        /// 判断短信内容是否是完全没有意义的乱码
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool IsFullGarbled(string strInput)
        {


            foreach (char c in strInput)
            {
                //如果包含短信中的一个字符，则不完全是没有意义的乱码
                if (char.IsLetterOrDigit(c) || HanziProxy.DicItems.ContainsKey(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
