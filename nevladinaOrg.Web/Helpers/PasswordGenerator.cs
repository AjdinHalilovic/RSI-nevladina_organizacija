using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace nevladinaOrg.Web.Helpers
{
    public static class PasswordGenerator
    {
        public static string GenerateRandomAlphanumericString(int length)
        {
            if (length <= 3)
                return string.Empty;

            var password = string.Empty;

            password += GetRandomString(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            password += GetRandomString(1, "0123456789");
            password += GetRandomString(1, "!#$%&/()=?");

            password += GetRandomString(length - 3, "abcdefghijklmnopqrstuvwxyz");

            return password;
        }

        private static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            var characterArray = characterSet.Distinct().ToArray();
            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            Random rnd = new Random();
            int randomNumber = rnd.Next(length);
            int randomNumber2 = rnd.Next(52, 61);
            for (int i = 0; i < length; i++)
            {
                if (i == randomNumber)
                {
                    ulong value = (ulong)randomNumber2;
                    result[i] = characterArray[value % (uint)characterArray.Length];
                }
                else
                {
                    ulong value = BitConverter.ToUInt64(bytes, i * 8);
                    result[i] = characterArray[value % (uint)characterArray.Length];
                }
            }
            return new string(result);
        }
    }
}
