using Common.Domain.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Cripto
{
    public class CryptoSymmetric : ICryptoSymmetric
    {
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        private byte[] Key;

        public string Encrypt(string value)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(this.Key, this.Key), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(value);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public string Decrypt(string value)
        {
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(this.Key, this.Key), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

        public void SetKey(string value)
        {
            this.Key = ASCIIEncoding.ASCII.GetBytes(value);
        }
    }
}
