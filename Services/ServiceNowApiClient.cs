using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Enums;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Models;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public class ServiceNowApiClient : IServiceNowApiClient
    {
        private readonly RestClient _client;
        private readonly ILogger _logger;
        private readonly RestClientOptions _restClientOptions;

        public ServiceNowApiClient(string baseUrl)
        {
            // Define file paths and filenames for encryption key and initialization vector (IV)

            //string sharedDrivePath = @"\\corefs.med.umich.edu\Shared2\MCIT_Shared\Teams\DES_ALL\DispoDataAssistant\ServiceNow";
            string sharedDrivePath = @"C:\Users\jsissom\ServiceNow";
            string encryptionKeyFileName = "EncryptionKey.txt";
            string ivFileName = "InitializationVector.txt";
            _restClientOptions = new RestClientOptions(baseUrl);

            EncryptionService encryptionService = new EncryptionService(sharedDrivePath, encryptionKeyFileName, ivFileName);
            var (serviceNowUsername, serviceNowPassword) = encryptionService.GetDecryptedServiceNowCredentials("ServiceNowUsername", "ServiceNowPassword");

            if (serviceNowUsername is not null && serviceNowPassword is not null)
            {
                _restClientOptions.Authenticator = new HttpBasicAuthenticator(serviceNowUsername.ToString(), serviceNowPassword.ToString());
            }
            
            _client = new RestClient(_restClientOptions);
            _logger = Ioc.Default.GetRequiredService<ILogger<ServiceNowApiClient>>();
        }

        public async Task<ServiceNowAsset?> GetServiceNowAssetAsync(string assetTag)
        {
            var request = new RestRequest("table/cmdb_ci", Method.Get);
            request.AddParameter("sysparm_query", $"asset_tag={assetTag}");
            request.AddParameter("sysparm_fields", "sys_id,asset_tag,model_id.name,manufacturer.name,serial_number,subcategory,sys_updated_on,operational_status,install_status");
            _logger.LogInformation($"Querying alm_hardware table from asset: {assetTag}");
            // The following line sends the request and automatically deserializes the response.
            RestResponse<ServiceNowResponse> response = await _client.ExecuteGetAsync<ServiceNowResponse>(request);

            // Check if the request was successful
            if (response.IsSuccessful && response.Data is not null)
            {
                _logger.LogInformation($"Query: {response.StatusCode}");
                ServiceNowResponse serviceNowResponse = response.Data;

                if (serviceNowResponse is not null && serviceNowResponse.Assets is not null && serviceNowResponse.Assets.Count > 0)
                {
                    _logger.LogInformation($"Asset: {assetTag} found");
                    return serviceNowResponse.Assets[0];
                }
                else
                {
                    _logger.LogWarning($"Asset: {assetTag} not found");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"Query: {response.ErrorMessage}");
                return null;
            }
        }

        public async Task<ServiceNowAsset?> RetireServiceNowAssetAsync(string sys_id)
        {
            object payload = new
            {
                install_status = ServiceNowInstallStatus.Retired.ToString(),
            };
            var request = new RestRequest($"table/cmdb_ci/{sys_id}");
            request.AddJsonBody(payload);
            
            RestResponse<ServiceNowResponse> response = await _client.ExecutePutAsync<ServiceNowResponse>(request);
            return response?.Data?.Assets?[0];
        }
    }
}
