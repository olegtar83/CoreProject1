using System;
using System.Linq;
using webapp.Abstractions;

namespace webapp.Services
{
    public class Encryption : IEncryption
    {
        private readonly byte [] _keySalt = new byte[] { 0x26, 0x19, 0x81, 0x4E, 0xA0, 0x6D, 0x95, 0x34, 0x26, 0x75, 0x64, 0x05, 0xF6 };

        public bool VerifyValueHash(string value, byte[] valueHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(_keySalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                if (computedHash.Where((t, i) => t != valueHash[i]).Any())
                {
                    return false;
                }
            }
            return true;
        }


        public string CreateValueHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(_keySalt))
            {
               var valueBytseHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(valueBytseHash);
            }
        }
    }
}
