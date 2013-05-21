using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Cmlu.Utility.Commons
{
    /// <summary>
    /// 使用静态方法扩展基本string类的功能
    /// </summary>
    public static class StringUtility
    {
        #region get系列函数
        /// <summary>
        /// 获取某个字符串a在另一个字符串b中出现的次数
        /// </summary>
        /// <param name="strOriginal">字符串b</param>
        /// <param name="strSymbol">字符串a</param>
        /// <returns>如果字符串a或b其中一个为null，则返回0</returns>
        public static int GetStrCount(string strOriginal, string strSymbol)
        {
            if (strOriginal == null || strSymbol == null)
            {
                return 0;
            }
            int count = 0;
            for (int i = 0; i < (strOriginal.Length - strSymbol.Length + 1); i++)
            {
                //这会使得交叉进行判断
                if (strOriginal.Substring(i, strSymbol.Length) == strSymbol)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取某个字符串a在另一个字符串b中第一次出现时前面所有的字符串
        /// </summary>
        /// <param name="strOriginal">字符串b</param>
        /// <param name="strSymbol">字符串a</param>
        /// <returns>如果字符串a，b有一个为null或空，则返回值为null;如果字符串b不包含字符串a，返回字符串b</returns>
        public static string GetBeforeFirstStr(string strOriginal, string strSymbol)
        {
            if (string.IsNullOrEmpty(strOriginal) || string.IsNullOrEmpty(strSymbol))
            {
                return null;
            }
            int strIndex = strOriginal.IndexOf(strSymbol);
            //字符串b不包含字符串a
            if (strIndex < 0)
            {
                return strOriginal;
            }

            return strOriginal.Substring(0, strIndex);
        }

        /// <summary>
        /// 获取字符串a在字符串b中第一次出现时后面所有的字符串
        /// </summary>
        /// <param name="strOriginal">字符串b</param>
        /// <param name="strSymbol">字符串a</param>
        /// <returns>如果字符串a，b有一个为null或空，则返回值为null;如果字符串b不包含字符串a，返回字符串b</returns>
        public static string GetAfterFirstStr(string strOriginal, string strSymbol)
        {
            if (string.IsNullOrEmpty(strOriginal) || string.IsNullOrEmpty(strSymbol))
            {
                return null;
            }

            int strIndex = strOriginal.IndexOf(strSymbol);
            //字符串b不包含字符串a
            if (strIndex < 0)
            {
                return strOriginal;
            }

            return strOriginal.Substring(strIndex + strSymbol.Length);
        }

        /// <summary>
        /// 获取字符串a在字符串b中最后一次出现之前的所有字符串
        /// </summary>
        /// <param name="strOriginal">字符串b</param>
        /// <param name="strSymbol">字符串a</param>
        /// <returns>如果字符串a，b有一个为null或空，则返回值为null;如果字符串b不包含字符串a，返回字符串b</returns>
        public static string GetBeforeLastStr(string strOriginal, string strSymbol)
        {
            if (string.IsNullOrEmpty(strOriginal) || string.IsNullOrEmpty(strSymbol))
            {
                return null;
            }

            int strPlace = strOriginal.LastIndexOf(strSymbol);
            //未能在字符串b中找到字符串a
            if (strPlace < 0)
            {
                return strOriginal;
            }

            return strOriginal.Substring(0, strPlace);
        }

        /// <summary>
        /// 获取字符串a在字符串b中最后一次出现时后面所有的字符串
        /// </summary>
        /// <param name="strOriginal">字符串b</param>
        /// <param name="strSymbol">字符串a</param>
        /// <returns>如果字符串a，b有一个为null或空，则返回值为null;如果字符串b不包含字符串a，返回字符串b</returns>
        public static string GetAfterLastStr(string strOriginal, string strSymbol)
        {
            if (string.IsNullOrEmpty(strOriginal) || string.IsNullOrEmpty(strSymbol))
            {
                return null;
            }

            int strIndex = strOriginal.LastIndexOf(strSymbol);
            //字符串b不包含字符串a
            if (strIndex < 0)
            {
                return strOriginal;
            }

            return strOriginal.Substring(strIndex + strSymbol.Length);
        }

        /// <summary>
        /// 将字符串按照分隔符转换成list
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="isToLower">是否转换为小写，默认为false</param>
        /// <returns></returns>
        public static List<string> GetStrList(string source, char speater, bool isToLower = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            List<string> list = new List<string>();
            string[] array = source.Split(speater);
            foreach (string item in array)
            {
                if (!string.IsNullOrEmpty(item) && item != speater.ToString())
                {
                    if (isToLower)
                    {
                        list.Add(item.ToLower());
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 将 List&lt;string&gt;按照分隔符组装成string 
        /// </summary>
        /// <param name="list">待转换的list</param>
        /// <param name="speater">分隔符</param>
        /// <returns></returns>
        public static string GetStrFromList(List<string> list, string speater)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    //最后一项之后 不需要再加分隔符
                    sb.Append(list[i]);
                }
                else
                {
                    //如果sb.Append(str)中的str为null，sb将不做修改
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 如果一个字符串为null，则返回string.Empty，否则返回原来的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetOriginalOrEmptyString(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            return input;
        }

        /// <summary>
        /// 获取两个字符串之间的字符串
        /// </summary>
        /// <param name="begin">起始</param>
        /// <param name="end">结束</param>
        /// <returns>begin第一次出现和end第一次出现之间的所有字符串，发生错误将返回null。</returns>
        public static string GetBetween(string source,string begin, string end)
        {
            if(string.IsNullOrEmpty(source) || string.IsNullOrEmpty(begin) || string.IsNullOrEmpty(end))
            {
                return null;
            }
            //判断是否包含begin字符串
            int beginIndex = source.IndexOf(begin);
            if (beginIndex < 0)
            {
                return null;
            }
            //判断是否包含end字符串
            int endIndex = source.IndexOf(end);
            if (endIndex < 0)
            {
                return null;
            }
            
            /*
             * 如果endIndex == beginIndex+begin.Length，则end紧跟在begin之后
             * 如果endIndex  < beginIndex+begin.Length, 则end和begin出现交叉，或end在begin之前
             * 如果endIndex  > beginIndex+begin.Length, 则end在begin之前出现
             */
            if (endIndex < beginIndex + begin.Length)
            {
                return null;
            }

            //取两个字符串之间的所有字符串
            return source.Substring(beginIndex + begin.Length, endIndex - beginIndex - begin.Length);
        }
        #endregion

        #region to系列函数
        /// <summary>
        /// 转全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                //全角空格12288，半角空格32
                if(array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }

                if (array[i] < 127)
                {
                    //全角和半角相差65248
                    array[i] = (char)(array[i]+65248);
                }
            }
            return new string(array);
        }

        /// <summary>
        /// 转半角函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    //全角空格转半角空格
                    array[i] = (char)32;
                    continue;
                }

                //从'！'到'~'为全角
                if (array[i] > 65280 && array[i] < 65375)
                {
                    //全角和半角相差65248（正好为全角'！'-'!'半角）
                    array[i] = (char)(array[i] - 65248);
                }

            }
            return new string(array);
        }
        #endregion

        #region 获取唯一字符串系列
        /// <summary>
        /// 根据GUID生成唯一的字符串id
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            Byte[] array = Guid.NewGuid().ToByteArray();
            foreach (byte b in array)
            {
                i *= ((int)b+1);
            }
            //将其格式化为16进制
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID生成唯一的数字id
        /// </summary>
        /// <returns></returns>
        public static long GenerateLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        #endregion

        #region 验证码相关

        /// <summary>
        /// 生成四位验证码，第一位数字，第二位字母，第三位数字，第四位字母
        /// 参数表示是否是小写
        /// </summary>
        /// <returns></returns>
        public static string YZM(bool isLower=false)
        {
            char[] nums = { '0','1','2','3','4','5','6','7','8','9' };
            char[] alphabet = { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };
            string result = string.Empty;
            Random rand = new Random();
            //返回小于传入参数的随机数
            result += nums[rand.Next(nums.Length)];
            result += isLower?alphabet[rand.Next(alphabet.Length)].ToString().ToLower():alphabet[rand.Next(alphabet.Length)].ToString().ToUpper();
            result += nums[rand.Next(nums.Length)];
            result += isLower ? alphabet[rand.Next(alphabet.Length)].ToString().ToLower() : alphabet[rand.Next(alphabet.Length)].ToString().ToUpper();
            return result;
        }

        #endregion

        #region is系列判断函数
        /// <summary>
        /// 判断一个字符串是否是回文
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>null，返回false；空返回true</returns>
        public static bool IsPalindrome(string input)
        {
            if (input == null)
            {
                return false;
            }

            bool result = true;
            //除以2，只需做一半的判断
            for (int i = 0; i < input.Length/2; i++)
            {
                if (input[i] != input[input.Length - 1 - i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
        #endregion

        #region 编辑距离

        #endregion
    }
}
