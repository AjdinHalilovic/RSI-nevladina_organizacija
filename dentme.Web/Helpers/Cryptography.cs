using System;
using System.Text;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace nevladinaOrg.Web.Helpers
{
    public static class Cryptography
    {
        public static class Hash
        {
            public static string Create(string value, string salt)
            {
                var valueBytes = KeyDerivation.Pbkdf2(
                    password: value,
                    salt: Encoding.UTF8.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);

                return Convert.ToBase64String(valueBytes);
            }

            public static bool Validate(string value, string salt, string hash) => Create(value, salt) == hash;
        }

        public static class Salt
        {
            public static string Create()
            {
                var randomBytes = new byte[128 / 8];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);
                }
            }
        }
    }
}
