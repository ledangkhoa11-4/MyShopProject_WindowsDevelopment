using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyShopProject.DTO
{
    public class Account : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;
        private string key;
        private string iv;
        public string Username {
            get { return string.Join("", username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { username = value; } 
        }    
        public string Password {
            get { return string.Join("", password.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { password = value; }
        }
        public string Key
        {
            get { return string.Join("", key.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { key = value; }
        }
        public string IV
        {
            get { return string.Join("", iv.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { iv = value; }
        }
        public static byte[] Encrypt(byte[] inputBytes, byte[] key, byte[] iv)
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(inputBytes, 0, inputBytes.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
        public static byte[] Decrypt(byte[] inputBytes, byte[] key, byte[] iv)
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream msDecrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] outputBytes = new byte[inputBytes.Length];
                        int decryptedByteCount = csDecrypt.Read(outputBytes, 0, outputBytes.Length);
                        return outputBytes.Take(decryptedByteCount).ToArray();
                    }
                }
            }
        }
    }

}
