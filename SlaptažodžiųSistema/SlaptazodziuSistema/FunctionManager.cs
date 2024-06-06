using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SlaptazodziuSistema
{
    public class FunctionManager
    {
        private static readonly string filePath = "passwords.txt";
        private static readonly string encryptionKey = "pAYjE6EDYPvh9WXDd4KmTG5vaKTLiYC6";

        // Functions
        public static void SavePassword(string name, string password, string urlOrApp, string comment)
        {
            try
            {
                var encryptedPassword = EncryptString(password, encryptionKey);
                var entry = $"{name}:{encryptedPassword}:{urlOrApp}:{comment}";

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(entry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving password: {ex.Message}");
            }
        }

        public static string SearchPassword(string name)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                var lines = File.ReadAllLines(filePath);
                return lines.FirstOrDefault(line => line.Split(':')[0] == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching password: {ex.Message}");
                return null;
            }
        }

        public static void UpdatePassword(string name, string newPassword)
        {
            try
            {
                if (!File.Exists(filePath)) return;

                var lines = File.ReadAllLines(filePath);
                var updatedLines = lines.Select(line =>
                {
                    var parts = line.Split(':');
                    if (parts[0] == name)
                    {
                        var encryptedPassword = EncryptString(newPassword, encryptionKey);
                        return $"{parts[0]}:{encryptedPassword}:{parts[2]}:{parts[3]}";
                    }
                    return line;
                }).ToArray();

                File.WriteAllLines(filePath, updatedLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex.Message}");
            }
        }

        public static void DeletePassword(string name)
        {
            try
            {
                if (!File.Exists(filePath))
                    return;
                var lines = File.ReadAllLines(filePath);
                var remainingLines = lines.Where(line => line.Split(':')[0] != name);
                File.WriteAllLines(filePath, remainingLines.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting password: {ex.Message}");
            }
        }

        // Encrypt string
        public static string EncryptString(string plainText, string keyString)
        {
            using (var aesAlg = Aes.Create())
            {
                using (var key = new Rfc2898DeriveBytes(keyString, Encoding.UTF8.GetBytes(keyString), 10000))
                {
                    aesAlg.Key = key.GetBytes(32); // 256-bit key
                    aesAlg.GenerateIV(); // Generate a new IV

                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        var iv = aesAlg.IV;
                        var encryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + encryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var aesAlg = Aes.Create())
            {
                using (var key = new Rfc2898DeriveBytes(keyString, Encoding.UTF8.GetBytes(keyString), 10000))
                {
                    aesAlg.Key = key.GetBytes(32); // 256-bit key

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        // Encrypt file
        public static void EncryptFile(string filePath, string key)
        {
            try
            {
                var text = File.ReadAllText(filePath);
                var encryptedText = EncryptString(text, key);
                File.WriteAllText(filePath, encryptedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting file: {ex.Message}");
            }
        }

        // Decrypt file
        public static void DecryptFile(string filePath, string key)
        {
            try
            {
                var encryptedText = File.ReadAllText(filePath);
                var decryptedText = DecryptString(encryptedText, key);
                File.WriteAllText(filePath, decryptedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting file: {ex.Message}");
            }
        }

        // Properties
        public static string Key => encryptionKey;
        public static string Path => filePath;
    }
}
