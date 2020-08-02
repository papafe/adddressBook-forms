using System.Linq;

namespace AddBook.Utilities
{
    public static class Validator
    {
        //Obviously the validators are not exactly production ready...
        public static bool IsEmail(string email)
        {
            return email?.Contains("@") == true;
        }

        public static bool IsPhoneNumber(string phone)
        {
            return phone?.Length > 2
                && phone?.All(char.IsDigit) == true;
        }

        public static bool IsNotEmpty(string val)
        {
            return !string.IsNullOrEmpty(val?.Trim());
        }

    }
}
