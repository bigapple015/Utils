using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace iFlyTek.ECSS30.Tool.SMSProc
{
    /// <summary>
    /// 短信预处理
    /// </summary>
    public class SMSPreProc
    {
        /// <summary>
        /// 简繁体转换表
        /// </summary>
        private static Hashtable theTrad2SimpTable;
        /// <summary>
        /// 停词
        /// </summary>
        private static string stopWords = "|/\\().,_-=+:~·;#:（）。`{}[]，—+：；‘’\'!！？?<>《》*&@$%、^…\"“”呢了吗";

        /// <summary>
        /// 归一化处理
        /// 1、去除前后空格
        /// 2、大写转小写
        /// 3、全角转半角
        /// 4、去除短信中间的空白字符
        /// 5、去除20以下的ASCII值
        /// 6、繁体转简体
        /// 7、去除停词
        /// 8、中文数字转数字
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static string PreProc(string strInput)
        {
            try
            {
                if (string.IsNullOrEmpty(strInput))
                {
                    return string.Empty;
                }
                StringBuilder sBuff = new StringBuilder();
                //大写转小写，去除前后空格
                strInput = strInput.Trim().ToLower();
                for (int i = 0; i < strInput.Length; i++)
                {
                    char c = strInput[i];
                    //去除中间的空白及ASCII码20以下的字符
                    if (c == '\t' || c == ' ' || char.IsWhiteSpace(c) || c < 20 || stopWords.Contains(c.ToString()))
                    {
                        continue;
                    }
                    //从'！'到'~'为全角
                    //全角空格12288，半角空格32
                    else if (c == '　')
                    {
                        c = ' ';
                    }
                    //'！'65281     '～'65374
                    else if (c >= '！' && c <= '～')
                    {
                        //全角和半角相差65248（正好为全角'！'-'!'半角）
                        //c = (char)(c - '！' + '!');
                        c = (char)(c - 65248);
                    }
                    else
                    {
                        string s = c.ToString();
                        //将繁体转换为简体
                        if (theTrad2SimpTable != null && theTrad2SimpTable.ContainsKey(s))
                        {
                            //國=>国，...
                            sBuff.Append(theTrad2SimpTable[s]);
                        }
                        else
                        {
                            sBuff.Append(s);
                        }
                    }
                }

                //将中文数字转换为阿拉伯数字
                return CovertChinaToNumber(sBuff.ToString());
            }
            catch (Exception ex)
            {
                return strInput;
            }
        }


        /// <summary>
        /// 初始化简繁体转换表
        /// </summary>
        /// <param name="strTrad2SimpFn">简繁体文件名</param>
        /// <returns></returns>
        public static void Init(string fileTrad2Simp)
        {
            try
            {
                //使用操作系统默认编码和\t作为分隔符
                Init(fileTrad2Simp, Encoding.Default, '\t');
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 初始化简繁体转换表
        /// 该表格式如下：
        ///職	职
        ///聹	聍
        ///聽	听
        ///聾	聋
        ///肅	肃
        /// </summary>
        /// <param name="fileTrad2Simp">简繁体文件名</param>
        /// <param name="encoder">文件要使用的字符编码</param>
        /// <param name="separtor">简繁体表的分隔符</param>
        /// <returns>是否初始化成功</returns>
        public static void Init(string fileTrad2Simp, Encoding encoder, params char[] separtor)
        {

            theTrad2SimpTable = new Hashtable();
            theTrad2SimpTable.Clear();

            using (StreamReader sr = new StreamReader(fileTrad2Simp, encoder))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                {
                    string[] sTemp = strLine.Split(separtor);

                    if (sTemp.Length != 2)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(sTemp[0]) && !theTrad2SimpTable.ContainsKey(sTemp[0]))
                    {
                        theTrad2SimpTable.Add(sTemp[0], sTemp[1]);
                    }
                }
            }

        }



        /// <summary>
        /// 将汉字对应数字添加到映射表
        /// </summary>
        internal static void Init()
        {
            for (int index = 0; index < _chinaToNumber.Length; index = index + 2)
            {
                if (!_htChinaToNumber.ContainsKey(_chinaToNumber[index]))
                {
                    _htChinaToNumber.Add(_chinaToNumber[index], _chinaToNumber[index + 1]);
                }
            }
        }

        private static string[] _chinaToNumber = new string[] 
        { 
            "一", "1", "壹", "1", 
            "二", "2", "两", "2", "贰", "2", 
            "三", "3", "叁", "3", 
            "四", "4", "肆", "4", 
            "伍", "5", "五", "5", 
            "六", "6", "陆", "6",
            "七", "7", "柒", "7",
            "八", "8", "捌", "8",
            "九", "9", "玖", "9" 
        };


        /// <summary>
        /// 用于将汉字中关于数字的说法转化成阿拉伯数字
        /// 如一百二十转换成120
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        internal static string CovertChinaToNumber(string strInput)
        {
            //初始化放到静态构造函数中去
            //Init();

            int number = 0;
            string newString = strInput;
            int begin = 0;   //strInput中需要转换的字符开始位置
            int newIndex = 0;   //替换后的字符串对应需要替换的开始位置
            int match = 0;
            for (int index = 0; index < strInput.Length; index++)
            {
                string strChina = strInput[index].ToString();
                //处理“十二”这种类型
                if ((strChina.Equals("十") || strChina.Equals("拾")) && match == 0)
                {
                    match = 1;
                    number = 10;
                }
                //匹配上数字 并且不能为“一个”、“一下”这样的词
                else if (_htChinaToNumber.ContainsKey(strChina) == true
                    && !IsSepecialWords(strInput, index))
                {

                    int intNumber = Convert.ToInt32(_htChinaToNumber[strChina]);
                    //匹配到结尾的字符
                    if (index + 1 == strInput.Length)
                    {
                        //这是最后一位，前面已经匹配上，只要直接相加，反之亦然
                        //如一百五十一，前面得出150，只需要加上一
                        if (match == 1)
                            number += intNumber;
                        else
                            number = number * 10 + intNumber;
                        if (number > 0)
                        {
                            newString = newString.Substring(0, newIndex) + number.ToString();
                        }
                    }
                    //匹配连续数字，如“一三九”邮箱
                    else if (match == 3)
                    {
                        number = number * 10 + intNumber;
                    }
                    else
                    {
                        strChina = strInput[index + 1].ToString();
                        if (_htChinaToNumber.ContainsKey(strChina))
                        {
                            number = number * 10 + intNumber;
                            match = 3;
                        }
                        else
                        {
                            switch (strInput[index + 1])
                            {
                                //对于万、百等直接计算，如三百则变成3*100=300
                                case '万':
                                    match = 1;
                                    index++;
                                    number += intNumber * 10000;
                                    break;
                                case '千':
                                    match = 1;
                                    index++;
                                    number += intNumber * 1000;
                                    break;
                                case '百':
                                    match = 1;
                                    index++;
                                    number += intNumber * 100;
                                    break;
                                case '十':
                                case '拾':
                                    match = 1;
                                    index++;
                                    number += intNumber * 10;
                                    break;
                                case '元':
                                case '块':
                                case '圆':
                                    match = 1;
                                    number += intNumber;
                                    break;
                                case '零':
                                    number += intNumber;
                                    match = 3;
                                    break;
                                default:
                                    number += intNumber;
                                    if (match == 0)
                                    {
                                        match = 4;
                                    }
                                    break;
                            }
                            if (index + 1 == strInput.Length)
                            {
                                newString = newString.Substring(0, newIndex) + number.ToString();
                            }
                        }
                    }

                }
                else if (strChina.Equals("零"))
                {
                    if (match == 0)
                    {
                        //处理以零开始的，如“零五五一”
                        string strTemp = newString;
                        newString = strTemp.Substring(0, newIndex) + number.ToString() + strTemp.Substring(newIndex + 1);
                        newIndex += 1;
                        begin = index + 1;
                    }
                    else if (match == 2 || match == 4)
                    {
                        number = 0;
                        if (match == 2)
                        {
                            newIndex += 2;
                        }
                        else
                        {
                            newIndex += 1;
                        }
                        begin = index + 1;
                        match = 0;
                    }
                    //3代表连续数字，如“一零零八六”，零需要特殊处理
                    else if (match == 3)
                    {
                        number = number * 10;
                    }
                    else
                        continue;
                }
                else
                {
                    //字符串的替换，目前替换时这样的，在原始的句子“十元的彩铃一百套餐”中，
                    //先替换成“10元的彩铃一百套餐”，Index会变化，因此需要保存要替换的字符的Index
                    if (match == 1 || match == 4 || match == 3)
                    {
                        string strTemp = newString;
                        if (newIndex + index - begin == newString.Length)
                        {
                            newString = strTemp.Substring(0, newIndex) + number.ToString();
                            newIndex += number.ToString().Length + 1;
                        }
                        else
                        {
                            newString = strTemp.Substring(0, newIndex) + number.ToString() + strTemp.Substring(newIndex + index - begin);
                            newIndex += number.ToString().Length + 1;
                        }

                        match = 0;
                    }
                    else if (match == 2)
                    {
                        newIndex += 2;
                        match = 0;
                    }
                    else
                    {
                        newIndex++;
                    }
                    begin = index + 1;
                    number = 0;
                    continue;
                }
            }
            return newString;
        }

        /// <summary>
        ///  如果短信中包含了“一个”或者“一下”或者“一些”，就认为是特殊短信
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool IsSepecialWords(string strInput, int index)
        {
            if (string.IsNullOrEmpty(strInput))
            {
                return false;
            }
            if (strInput.Length <= index)
            {
                return false;
            }
            if (strInput[index].ToString().Equals("一"))
            {
                return ((index - 1 < 0) || !_htChinaToNumber.ContainsKey(strInput[index - 1].ToString()))
                    && (index + 1) < strInput.Length &&
                    (strInput[index + 1].ToString().Equals("下") || strInput[index + 1].ToString().Equals("个") || strInput[index + 1].ToString().Equals("些"));

            }
            return false;
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SMSPreProc()
        {
            Init();
        }

        /// <summary>
        /// 汉字和数字的对应列表
        /// </summary>
        private static Dictionary<string, string> _htChinaToNumber = new Dictionary<string, string>();

    }
}