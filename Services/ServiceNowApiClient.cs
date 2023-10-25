using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public class ServiceNowApiClient : IServiceNowApiClient
    {
        private readonly RestClient _client;

        public ServiceNowApiClient(string baseUrl)
        {
            // Define file paths and filenames for encryption key and initialization vector (IV)

            //string sharedDrivePath = @"\\corefs.med.umich.edu\Shared2\MCIT_Shared\Teams\DES_ALL\DispoDataAssistant\ServiceNow";
            string sharedDrivePath = @"C:\Users\jsissom\ServiceNow";
            string encryptionKeyFileName = "EncryptionKey.txt";
            string ivFileName = "InitializationVector.txt";

            EncryptionService encryptionService = new EncryptionService(sharedDrivePath, encryptionKeyFileName, ivFileName);
            var (serviceNowUsername, serviceNowPassword) = encryptionService.GetDecryptedServiceNowCredentials("ServiceNowUsername", "ServiceNowPassword");
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(serviceNowUsername, serviceNowPassword)
            };
            _client = new RestClient(options);
        }

        public async Task<ServiceNowAsset?> GetServiceNowAssetAsync(string assetTag)
        {
            var request = new RestRequest("table/alm_hardware", Method.Get);
            request.AddParameter("sysparm_query", $"asset_tag={assetTag}");
            request.AddParameter("sysparm_fields", "asset_tag,model.name,model.manufacturer.name,serial_number,model_category.name");

            // The following line sends the request and automatically deserializes the response.
            RestResponse<ServiceNowResponse> response = await _client.ExecuteAsync<ServiceNowResponse>(request);

            // Check if the request was successful
            if (response.IsSuccessful)
            {
                ServiceNowResponse serviceNowResponse = response.Data;

                if (serviceNowResponse != null && serviceNowResponse.Assets.Count > 0)
                {
                    return serviceNowResponse.Assets[0];
                }
            }

            return null;
        }
    }
}
