using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace AppAspNetCore.Extentions
{
    public static class HashMD5
    {
        public static string ToMD5(this string str) 
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (var b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }

            return bHash.ToString();
        }
    }
}