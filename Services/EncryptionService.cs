using DispoDataAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public class EncryptionService
    {
        private readonly string sharedDrivePath;
        private readonly string encryptionKeyFileName;
        private readonly string ivFileName;

        public EncryptionService(string sharedDrivePath, string encryptionKeyFileName, string ivFileName)
        {
            this.sharedDrivePath = sharedDrivePath;
            this.encryptionKeyFileName = encryptionKeyFileName;
            this.ivFileName = ivFileName;
        }

        public (string Username, string Password) GetDecryptedServiceNowCredentials(string usernameKey, string passwordKey)
        {
            string serviceNowUsername = GetSecretKey(usernameKey);
            string serviceNowPassword = GetSecretKey(passwordKey);

            // Read the base64-encoded encryption key and initialization vector (IV) from the shared drive
            string base64EncryptionKey = FileHelper.ReadText(Path.Combine(sharedDrivePath, encryptionKeyFileName));
            string base64Iv = FileHelper.ReadText(Path.Combine(sharedDrivePath, ivFileName));

            // Convert the base64-encoded encryption key and IV to byte arrays
            byte[] encryptionKey = Convert.FromBase64String(base64EncryptionKey);
            byte[] iv = Convert.FromBase64String(base64Iv);

            // Decrypt username and password
            serviceNowUsername = Decrypt(serviceNowUsername, encryptionKey, iv);
            serviceNowPassword = Decrypt(serviceNowPassword, encryptionKey, iv);

            return (serviceNowUsername, serviceNowPassword);
        }

        private string GetSecretKey(string key)
        {
            // Implement your logic to retrieve the secret key, e.g., from configuration.
            // You can use dependency injection to provide a configuration service.
            // For example, return ConfigurationManager.AppSettings[key] in a non-ASP.NET Core application.
            // In an ASP.NET Core application, you can use IConfiguration.
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                return "Error reading app settings";
            }
        }

        private string Decrypt(string secret, byte[] encryptionKey, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = encryptionKey;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedBytes = Convert.FromBase64String(secret);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
