using Traffic.Utilities.Constants;
using System;
using System.Globalization;
using System.Threading;

namespace Traffic.Utilities.Helpers
{
    public static class DateTimeHelper
    {
        public static int GetWorkingDays(DateTime? from, DateTime to)
        {
            if (from == null)
            {
                from = DateTime.Now.Date;
            }
            else
            {
                from = from.Value.Date;
            }

            to = to.Date;

            var totalDays = 0;

            for (var date = from.Value; date < to; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
            }

            return totalDays;
        }

        public static string GetShortDatePattern(string dateStr)
        {
            var tmp = dateStr.Split(" ");
            if (tmp.Length > 1)
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                var dateTimeFormat = currentCulture.DateTimeFormat;
                var shortDatePattern = dateTimeFormat.ShortDatePattern;

                return shortDatePattern;
            }

            return CommonConstants.DateFormat;
        }
    }
}
