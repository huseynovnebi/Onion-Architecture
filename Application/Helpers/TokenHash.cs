using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class TokenHash
    {
        public static string GetHash(byte[] tokenBytes,byte[] saltBytes,int IterationCount,int HashSize)
        {
            string tokenhash = "";
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(tokenBytes, saltBytes, IterationCount))
            {
                byte[] hashData = rfc2898DeriveBytes.GetBytes(HashSize);
                tokenhash = Convert.ToBase64String(hashData);
            }
            return tokenhash;
        }
    }
}
