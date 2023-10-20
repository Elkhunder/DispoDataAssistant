using DispoDataAssistant.Helpers;
using DispoDataAssistant.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DispoDataAssistant.Handlers
{
    public class ServiceNowHandler
    {
        private HttpClient _client;
        public ServiceNowHandler()
        {
            _client = new HttpClient();
            string sharedDrivePath = @"\\corefs.med.umich.edu\Shared2\MCIT_Shared\Teams\DES_ALL\DispoDataAssistant\ServiceNow";
            string encryptionKeyFileName = "EncryptionKey.txt";
            string ivFileName = "InitializationVector.txt";

            string serviceNowUsername = GetSecretKey("ServiceNowUsername");
            string serviceNowPassword = GetSecretKey("ServiceNowPassword");

            string base64EncryptionKey = FileHelper.ReadText(Path.Combine(sharedDrivePath, encryptionKeyFileName));
            string base64Iv = FileHelper.ReadText(Path.Combine(sharedDrivePath, ivFileName));
            byte[] encryptionKey = Convert.FromBase64String(base64EncryptionKey);
            byte[] iv = Convert.FromBase64String(base64Iv);

            
            

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = encryptionKey;
                aes.IV = iv;

                // Decrypt Username
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedUsernameBytes = Convert.FromBase64String(serviceNowUsername);
                    byte[] decryptedUsernameBytes = decryptor.TransformFinalBlock(encryptedUsernameBytes, 0, encryptedUsernameBytes.Length);
                    serviceNowUsername = Encoding.UTF8.GetString(decryptedUsernameBytes);
                }

                // Decrypt Password
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedPasswordBytes = Convert.FromBase64String(serviceNowPassword);
                    byte[] decryptedPasswordBytes = decryptor.TransformFinalBlock(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
                    serviceNowPassword = Encoding.UTF8.GetString(decryptedPasswordBytes);
                }
            }


            

            var creds = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{serviceNowUsername}:{serviceNowPassword}"));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);
        }   

        public async Task<ServiceNowAsset?> GetServiceNowAssetAsync(string assetTag)
        {
            HttpResponseMessage response = await _client.GetAsync($"https://ummeddev.service-now.com/api/now/table/alm_hardware?sysparm_query=asset_tag={assetTag}&sysparm_fields=asset_tag,model.name,model.manufacturer.name,serial_number,model_category.name");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseBody))
            {
                ServiceNowAsset[] assets = JsonSerializer.Deserialize<ServiceNowAsset[]>(JsonDocument.Parse(responseBody).RootElement.GetProperty("result").GetRawText());
                
                if (assets is not null && assets.Length > 0)
                {
                    return assets[0];
                }
            }
            return null;
        }

        private string GetSecretKey(string key)
        {
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

        
    }
}