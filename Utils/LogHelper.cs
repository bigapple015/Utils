using System;
using System.Collections.Generic;
using System.Text;

namespace 词典相关工具
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 写信息
        /// </summary>
        /// <param name="msg"></param>
        public static void LogI(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Info:\t"+msg);
            }
        }

        /// <summary>
        /// 写调试
        /// </summary>
        /// <param name="msg"></param>
        public static void LogD(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Debug:\t" + msg);
            }
        }

        /// <summary>
        /// 写警告
        /// </summary>
        /// <param name="msg"></param>
        public static void LogW(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Warning:\t" + msg);
            }
        }

        /// <summary>
        /// 写错误
        /// </summary>
        /// <param name="msg"></param>
        public static void LogE(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Error:\t" + msg);
            }
        }

    }
}
