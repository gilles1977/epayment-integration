using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Common.Utils
{
    public class CryptoUtils
    {
        private static byte[] Hash(string str, string salt, HashAlgorithm algorithm, Encoding encoding)
        {
            if (str == null || algorithm == null || encoding == null)
            {
                throw new ArgumentException("Either password, algorithm or encoding is null");
            }
            string passWithSalt = str + salt;
            byte[] passToByte = encoding.GetBytes(passWithSalt);
            byte[] hash = algorithm.ComputeHash(passToByte);

            return hash;
        }

        private static byte[] Encrypt(string str, string passPhrase, string salt, SymmetricAlgorithm algorithm)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            algorithm.Padding = PaddingMode.None;
            using (algorithm)
            {
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltBytes))
                {
                    algorithm.Key = password.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = password.GetBytes(algorithm.BlockSize / 8);
                    using (var memStream = new MemoryStream())
                    {
                        using (
                            var cryptoStream = new CryptoStream(memStream, algorithm.CreateEncryptor(),
                                                                CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(strBytes, 0, strBytes.Length);
                            return memStream.ToArray();
                        }
                    }
                }
            }
        }

        private static string Decrypt(byte[] cipher, string passPhrase, string salt, SymmetricAlgorithm algorithm)
        {
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            algorithm.Padding = PaddingMode.None;
            using (algorithm)
            {
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltBytes))
                {
                    algorithm.Key = password.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = password.GetBytes(algorithm.BlockSize / 8);
                    using (var memStream = new MemoryStream(cipher))
                    {
                        using (
                            var cryptoStream = new CryptoStream(memStream, algorithm.CreateDecryptor(),
                                                                CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cryptoStream))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public static string EncryptAes(string str, string passPhrase, string salt)
        {
            return Convert.ToBase64String(Encrypt(str, passPhrase, salt, new AesManaged()));
        }

        public static string DecryptAes(string str, string passPhrase, string salt)
        {
            return Decrypt(Convert.FromBase64String(str), passPhrase, salt, new AesManaged());
        }

        public static string HashPassword(string password)
        {
            return Convert.ToBase64String(Hash(password, "1234", new SHA256Managed(), new UTF8Encoding()));
        }

        public static string GetHashToken(string str)
        {
            return Convert.ToBase64String(Hash(str, "6543", new SHA256Managed(), new UTF8Encoding()));
        }

        public static string GetSha1Signature(string str)
        {
            var hash = Hash(str, string.Empty, new SHA1Managed(), new UTF8Encoding());
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
