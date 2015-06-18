using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using JasonSoft.Math;

namespace JasonSoft
{
    public static class StringHelper
    {
        public static String GetRandom(Int32 maxLength)
        {
            if (maxLength <= 0) throw new ArgumentException("Must bigger than 0", "maxLength");

            StringBuilder result = new StringBuilder(maxLength);

            for (int i = 0; i < maxLength; i++)
            {
                
                result.Append(48.Random(122, new List<Int32> {58, 59, 60, 61, 62, 63, 64, 91, 92, 93, 94, 95, 96}).ToASCII().ToString());
            }

            return result.ToString();
        }

        public static string GetRandomKey(this Int32 bytelength)
        {
            int len = bytelength * 2;
            byte[] buff = new byte[len / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            return sb.ToString();
        }
    }
}
