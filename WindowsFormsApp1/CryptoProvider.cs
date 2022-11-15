using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WindowsFormsApp1
{
    internal class CryptoProvider
    {
        private const int keySize = 256;
        private const int derivationIterations = 1000;
        public static string encryptFile(string text, string password)
        {
            byte[] salt = generateRandomBytes();
            byte[] iv = generateRandomBytes();
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            using (var shaKey = new Rfc2898DeriveBytes(password, salt, derivationIterations))
            {
                var keyBytes = shaKey.GetBytes(keySize / 8);
                using (ARC4Managed rc4 = new ARC4Managed())
                {
                    using (var encryptor = rc4.CreateEncryptor(keyBytes, iv))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(textBytes, 0, textBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = salt;
                                cipherTextBytes = cipherTextBytes.Concat(iv).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        public static string decryptFile(string encryptedText, string password)
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedText);
            var salt = cipherTextBytes.Take(keySize / 8).ToArray();
            var iv = cipherTextBytes.Skip(keySize / 8).Take(keySize / 8).ToArray();
            var textBytes = cipherTextBytes.Skip((keySize / 8) * 2).Take(cipherTextBytes.Length - ((keySize / 8) * 2)).ToArray();
            using (var shaKey = new Rfc2898DeriveBytes(password, salt, derivationIterations))
            {
                var keyBytes = shaKey.GetBytes(keySize / 8);
                using (ARC4Managed rc4 = new ARC4Managed())
                {
                    using (var decryptor = rc4.CreateDecryptor(keyBytes, iv))
                    {
                        using (var memoryStream = new MemoryStream(textBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainText = new byte[textBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainText, 0, plainText.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainText, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
        private static byte[] generateRandomBytes()
        {
            var randomBytes = new byte[32];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
                return randomBytes;
        }
    }
}
