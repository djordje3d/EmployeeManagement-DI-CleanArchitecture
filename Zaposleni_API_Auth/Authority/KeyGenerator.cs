using System;
using System.Security.Cryptography;

namespace Zaposleni_API_Auth.Authority
{
    public class KeyGenerator
    {
        public static string GenerateSecretKey(int size = 32)
        {
            var key = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(key);
        }
    }
}
