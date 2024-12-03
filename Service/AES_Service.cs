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
            // Create an Aes object with proper disposal
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            // Convert the plain text to a byte array using UTF8 encoding
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                            // Write the byte array directly to the CryptoStream
                            csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                            csEncrypt.FlushFinalBlock();
                        }

                        // Get the encrypted bytes
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
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

            // Create an Aes object with proper disposal
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                            {
                                // Read to end and ensure proper stream closing
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return plaintext;
        }

        public byte[] generateIV()
        {
            Aes aesAlg = Aes.Create();
            
           aesAlg.GenerateIV();
           byte[] iv = aesAlg.IV;
            return iv;
        }
    }
}
