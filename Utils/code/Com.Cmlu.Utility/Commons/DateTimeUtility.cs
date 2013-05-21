using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Cmlu.Utility.Commons
{
    /// <summary>
    /// 日期工具类，作为对Datetime类型的扩充
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        /// 给一个正整数，判断他表示的年份是不是闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            if (year <= 0)
            {
                throw new Exception("Input Parameter must be bigger than 0");
            }

            //能被4整除，不能被100整除， 
            //或者能被400整除
            if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定的月的天数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDays(DateTime dt)
        {
            switch (dt.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    if (DateTime.IsLeapYear(dt.Year))
                    {
                        //瑞年2月有29天
                        return 29;
                    }
                    else
                    {
                        return 28;
                    }
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
            }

            throw new Exception("It must be something wrong.");
        }

        /// <summary>
        /// 得到月末一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime dt)
        {
            int days = GetDays(dt);
            DateTime time = new DateTime(dt.Year, dt.Month, days);
            return time;
        }

        /// <summary>
        /// 得到月的第一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime dt)
        {
            DateTime time = new DateTime(dt.Year, dt.Month, 1);
            return time;
        }

        /// <summary>
        /// 判断当前时间是本周的第几天 星期1 - 1
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDayOfWeek(DateTime dt)
        {
            //因为在DayOfWeek枚举中的第一个字段为Sunday
            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return 7;
            }

            return (int)dt.DayOfWeek;
        }

        /// <summary>
        /// 获取本周的第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstdayOfWeek(DateTime dt)
        {
            int day = (int)dt.DayOfWeek;
            //获取当前日期，仅到天
            DateTime time = new DateTime(dt.Year, dt.Month, dt.Day);
            
            if (day > 0)
            {
                return time.AddDays(1 - day);
            }
            else
            {
                //处理等于0的情况，即星期天
                return time.AddDays(-6);
            }
        }

        /// <summary>
        /// 获取一日的开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetBeginTimeOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        /// <summary>
        /// 得到一日的最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndTimeOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        /// <summary>
        /// 获取两个时间之间的时间间隔
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static TimeSpan DateDiff(DateTime dt1, DateTime dt2)
        {
            return dt1 - dt2;
        }

        #region 格式化时间
        
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormateDate(DateTime dt)
        {
/*
f 秒的小数精度为一位。其余数字被截断。 
ff 秒的小数精度为两位。其余数字被截断。 
fff 秒的小数精度为三位。其余数字被截断。 
ffff 秒的小数精度为四位。其余数字被截断。 
fffff 秒的小数精度为五位。其余数字被截断。 
ffffff 秒的小数精度为六位。其余数字被截断。 
fffffff 秒的小数精度为七位。其余数字被截断。 
最多7位
*/
            return dt.ToString("yyyyMMddHHmmssffff");
        }

        #endregion
    }
}
