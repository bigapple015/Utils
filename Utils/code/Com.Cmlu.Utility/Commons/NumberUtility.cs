using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Cmlu.Utility.Commons
{
    /// <summary>
    /// 数字处理
    /// </summary>
    public static class NumberUtility
    {
        /// <summary>
        /// 求两个正数的最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GCD(int a, int b)
        {
            //利用公式 gcd(a,b) = gcd(b,a mod b)
            if (a < 0 || b < 0)
            {
                throw new Exception("input parameter must be bigger than or equal zero");
            }

            int temp;

            //使得大数放到a上
            if (a < b)
            {
                temp = b;
                b = a;
                a = temp;
            }

            while (b != 0)
            {
                temp = a % b;
                a = b;
                b = temp;
            }

            return a;
        }

        /// <summary>
        /// 求两个正数的最小公倍数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int LCM(int a, int b)
        {
            if (a <= 0 || b <= 0)
            {
                throw new Exception("input parameter must be bigger than 0");
            }
            //最小公倍数等于两数之积除以最大公约数
            return a * b / GCD(a, b);
        }
    }
}
