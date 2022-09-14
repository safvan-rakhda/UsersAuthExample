using System;
using System.Security.Cryptography;
using System.Text;

namespace UsersAuthExample.Auth
{
    public static class Encryption
    {
        public static string ComputePasswordHash(string password, string secrect)
        {
            using var mac = new HMACSHA256(Encoding.UTF8.GetBytes(secrect));
            return BytesToString(mac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public static string CreateSecret()
        {
            byte[] secret = new byte[128];
            new Random().NextBytes(secret);
            return BytesToString(secret);
        }

        private static string BytesToString(byte[] bytes)
        {
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
