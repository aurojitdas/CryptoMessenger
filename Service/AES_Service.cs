using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace test_wpf
{
    class AES_Service
    {

        public byte[] encrypt(string plainText, byte[] Key, byte[] IV)
        {

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            byte[] encrypted;
            Aes aesAlg = Aes.Create();

            aesAlg.Key = Key;
            aesAlg.IV = IV;
            
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            StreamWriter swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(plainText);

            encrypted = msEncrypt.ToArray();

            return encrypted;

        }

        public string decrypt(byte[] cipherText, byte[] Key, byte[] IV) 
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            string plaintext = null;

            Aes aesAlg = Aes.Create();

            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


            MemoryStream msDecrypt = new MemoryStream(cipherText);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            StreamReader srDecrypt = new StreamReader(csDecrypt);
            plaintext = srDecrypt.ReadToEnd();
            return plaintext;
        }
    }
}
