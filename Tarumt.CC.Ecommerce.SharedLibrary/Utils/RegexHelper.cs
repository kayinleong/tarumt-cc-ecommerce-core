using System.Text.RegularExpressions;

namespace Ky.Web.SharedLibrary.Utils
{
    public static partial class RegexHelper
    {
        public static bool ValidateMalaysianPhoneNumber(string phoneNumber)
        {
            Regex regex = MalaysiaPhoneNumberRegex();
            return regex.IsMatch(phoneNumber);
        }

        public static bool ValidateEmail(string email)
        {
            Regex regex = EmailRegex();
            return regex.IsMatch(email);
        }

        [GeneratedRegex("^[0-9]{9,10}$")]
        private static partial Regex MalaysiaPhoneNumberRegex();

        [GeneratedRegex("^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$")]
        private static partial Regex EmailRegex();
    }
}
