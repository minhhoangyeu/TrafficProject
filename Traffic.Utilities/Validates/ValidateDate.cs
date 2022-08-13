using Traffic.Utilities.Constants;
using Traffic.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Traffic.Utilities.Validates
{
    public class ValidateDate
    {
        public static bool CheckFormatDDMMYYYY(string dateStr)
        {
            try
            {
                var shortDatePattern = DateTimeHelper.GetShortDatePattern(dateStr);
                dateStr = dateStr.Split(" ")[0];
                var tmp = DateTime.ParseExact(dateStr, shortDatePattern, CultureInfo.InvariantCulture).ToString(CommonConstants.DateFormat);
                DateTime.ParseExact(tmp, CommonConstants.DateFormat, CultureInfo.InvariantCulture);
                return true;
                
            }
            catch
            {
                return false;
            }
        }
    }
}
