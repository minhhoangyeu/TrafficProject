using System.Text.RegularExpressions;

namespace Traffic.Utilities.Helpers
{
    public static class PhoneNumberHelper
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var pattern = @"^0[0-9]{9}$";
            Regex rgx = new Regex(pattern);

            var isValid = rgx.IsMatch(phoneNumber);
            return isValid;
        }
    }
}
