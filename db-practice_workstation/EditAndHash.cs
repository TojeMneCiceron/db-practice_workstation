using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace arm
{
    class EditAndHash
    {
        private static readonly Regex loginRegex = new Regex("^[A-Za-zА-ЯЁа-яё0-9 `~!@#№;%:&-_=/,\\t\\r\\v\\f\\n\\.\\$\\^\\{\\}\\[\\]\\(\\|\\)\\*\\+\\?\\\"\\\\]{1,50}$");

        public static string DeleteExtraSpaces(string str)
        {
            return Regex.Replace(DeleteBorderSpaces(str), "\\s+", " ");
        }

        public static string DeleteBorderSpaces(string str)
        {
            if (str is null)
                return "";
            return Regex.Replace(Regex.Replace(str, "^\\s+", ""), "\\s+$", "");
        }

        public static bool CheckPassword(string password)
        {
            int spacesCount = Regex.Matches(password, "\\s").Count;
            return password.Length - spacesCount >= 8;
        }

        public static bool CheckLogin(string login)
        {
            return loginRegex.IsMatch(login);
        }

        public static string Salt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public static string Hash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool Verify(string password, string dbhash)
        {
            return BCrypt.Net.BCrypt.Verify(password, dbhash);
        }
    }
}
