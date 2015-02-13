using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace 电报解析.Utils
{
    public class DateTimeUtils
    {
        public static bool TryParse(string s, out DateTime result)
        {
            if (!DateTime.TryParseExact(s, new string[] { "yyyy-MM-dd HHmmss", "yyyy-MM-dd HHmm" }, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return  false;
            }

            return true;
        }

        public static bool TryParse(string s, out TimeSpan result)
        {
            if (!TimeSpan.TryParseExact(s, new string[] {"hhmm", "hh mm"}, CultureInfo.CurrentCulture,
                    TimeSpanStyles.None, out result))
            {
                return false;
            }

            return true;
        }
    }
}
