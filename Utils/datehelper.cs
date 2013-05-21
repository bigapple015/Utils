//以下的毫秒都采用最大997，而不是999 因为SQL SERVER的精度为3毫秒
//本月的天数
int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

//本年的天数 是否是闰年           
int daysInYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;

//本月第一天
DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
//本月的最后一天 本月1号加一个月得下月1号，再剪掉一天就是本月最后一天
DateTime lastDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
//本月最后一天的午夜
DateTime lastDayInMonth2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddMilliseconds(-3);

//本年第一天
DateTime firstDayInYear = new DateTime(DateTime.Now.Year, 1, 1);

//本年最后一天
DateTime lastDayInYear = new DateTime(DateTime.Now.Year, 12, 31);
//本年最后一天的午夜
DateTime lastDayInYear2 = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59, 997);

//得到星期几 星期天为7
int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek) < 1 ? 7 : Convert.ToInt32(DateTime.Now.DayOfWeek);
//本周一
DateTime monday = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(1 - dayOfWeek);
//本周 星期天
DateTime sunday = monday.AddDays(6);
//本周 星期天的午夜
DateTime sunday2 = monday.AddDays(7).AddMilliseconds(-3);

//本季度第一天
DateTime firsyDayInQuarter = new DateTime(DateTime.Now.Year, DateTime.Now.Month - (DateTime.Now.Month - 1) 

