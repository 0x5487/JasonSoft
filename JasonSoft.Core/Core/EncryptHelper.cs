using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JasonSoft
{
    // Reference :
    // http://www.dotblogs.com.tw/chhuang/archive/2008/04/25/3485.aspx
    // http://kiranpatils.wordpress.com/2008/03/13/encryptiondecryption-helper-class-using-rijandelmanaged/

    public static class EncryptHelper
    {
        private const string _privateKey = "123456789@world@hello";
        private const string _salt = "jasonsoft";

        public static string Encrypt(string plaintext)
        {
            return Encrypt(plaintext, _salt);
        }

        public static string Encrypt(String plaintext, String salt)
        {
            Rfc2898DeriveBytes KeyBytes = new Rfc2898DeriveBytes(_privateKey, Encoding.UTF8.GetBytes(salt));
            RijndaelManaged alg = new RijndaelManaged();
            alg.Key = KeyBytes.GetBytes(32);
            alg.IV = KeyBytes.GetBytes(16);
            MemoryStream encryptStream = new MemoryStream();
            //Stream to write
            CryptoStream encrypt = new CryptoStream(encryptStream, alg.CreateEncryptor(), CryptoStreamMode.Write);
            //convert plain text to byte array
            byte[] data = Encoding.UTF8.GetBytes(plaintext);
            encrypt.Write(data, 0, data.Length); //data to encrypt,start,stop
            encrypt.FlushFinalBlock();//Clear buffer
            encrypt.Close();
            return Convert.ToBase64String(encryptStream.ToArray());//return encrypted data

        }

        public static string Decrypt(String ciphertext)
        {
            return Decrypt(ciphertext, _salt);
        }

        public static String Decrypt(String ciphertext, String salt)
        {
            Rfc2898DeriveBytes KeyBytes = new Rfc2898DeriveBytes(_privateKey, Encoding.UTF8.GetBytes(salt));
            //The deafault iteration count is 1000
            RijndaelManaged alg = new RijndaelManaged();
            alg.Key = KeyBytes.GetBytes(32);
            alg.IV = KeyBytes.GetBytes(16);
            MemoryStream decryptStream = new MemoryStream();
            //Stream to read
            CryptoStream decrypt = new CryptoStream(decryptStream, alg.CreateDecryptor(), CryptoStreamMode.Write);
            //convert  ciphertext to byte array
            byte[] data = Convert.FromBase64String(ciphertext); //IF using for WEB APPLICATION and getting ciphertext via Querystring change code to : Convert.FromBase64String(ciphertext.Replace(」 「,」+」));

            decrypt.Write(data, 0, data.Length); //data to encrypt,start,stop
            decrypt.Flush();
            decrypt.Close();
            return Encoding.UTF8.GetString(decryptStream.ToArray());//return PlainText
        }



    }
}
