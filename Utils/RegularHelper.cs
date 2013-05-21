using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyStock.com.lib
{
    public class RegularHelper
    {
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private RegularHelper()
        {
        }

        /// <summary>
        /// 验证电话号码的主要代码
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证手机号码的主要代码
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsMobile(string str_handset)

        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,5]+\d{9}");
        }

        /// <summary>
        /// 验证身份证号的主要代码
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDCard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"\d{17}[\d|X]|\d{15}");
        }

        /// <summary>
        /// 验证邮箱的主要代码
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsMail(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }


        /// <summary>
        /// 验证输入为数字的主要代码
        /// </summary>
        /// <param name="str_number"></param>
        /// <returns></returns>
        public static bool IsNumber(string str_number)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
        }

        /// <summary>
        /// 验证邮编的主要代码
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }

        /// <summary>
        /// 用于验证IP地址的有效性
        /// </summary>
        /// <param name="str_ip"></param>
        /// <returns></returns>
        public static bool IsIP(string str_ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_ip, @"\d+\.\d+\.\d+\.\d+");
        }

        /// <summary>
        /// 用于验证url网址的有效性
        /// </summary>
        /// <param name="str_url"></param>
        /// <returns></returns>
        public static bool IsURL(string str_url)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_url, @"[a-zA-z]+://[^\s]*");
        }

        /// <summary>
        /// 用于验证正整数的有效性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[1-9]\d*$");
        }

        /// <summary>
        /// 用于验证负整数的有效性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNegativeInteger(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^-[1-9]\d*$ ");
        }

        /// <summary>
        /// 用于验证整数的有效性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?[1-9]\d*$");
        }

        /// <summary>
        /// 匹配非负整数（正整数 + 0）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNonnegativeInteger(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[1-9]\d*|0$");
        }

        /// <summary>
        /// 匹配非正整数（负整数 + 0）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNonpositiveInteger(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^-[1-9]\d*|0$");
        }

        /// <summary>
        /// 匹配由数字和26个英文字母组成的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumAlphabet(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 匹配由26个英文字母的大写组成的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUppercase(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[A-Z]+$");
        }

        /// <summary>
        /// 匹配由26个英文字母的小写组成的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLowercase(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[a-z]+$");
        }

        /// <summary>
        /// 匹配由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumAlphabetUnderline(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\w+$");
        }
    }
}