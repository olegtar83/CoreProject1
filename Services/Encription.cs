using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp
{
    public class Encription : IEncription
    {
        private byte [] keySalt = new byte[] { 0x26, 0x19, 0x81, 0x4E, 0xA0, 0x6D, 0x95, 0x34, 0x26, 0x75, 0x64, 0x05, 0xF6 };

        public bool VerifyValueHash(string value, byte[] valueHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(keySalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != valueHash[i])
                        return false;
                }
            }
            return true;
        }

        public void CreateValueHash(string password, out string valueHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
               var valueBytseHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                valueHash= Convert.ToBase64String(valueBytseHash);
            }
        }
    }
}
