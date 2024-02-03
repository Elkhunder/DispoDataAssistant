using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Services.Implementations;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services;

public class ServiceNowApiClient : BaseService, IServiceNowApiClient
{
    private readonly RestClient _client;
    private readonly RestClientOptions _restClientOptions;
    private LifecycleMembers? lifecycleMembers;

    public ServiceNowApiClient(string baseUrl, ILogger logger) : base(logger)
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
    }

    public async Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetByAssetTagAsync(string assetTag)
    {
        var request = new RestRequest("table/alm_hardware", Method.Get);
        request.AddParameter("sysparm_query", $"asset_tag={assetTag}");
        request.AddParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name");
        _logger.LogInformation($"Querying alm_hardware table from asset: {assetTag}");
        // The following line sends the request and automatically deserializes the response.
        return await _client.ExecuteGetAsync<ServiceNowApiResponse>(request);
    }

    public async Task<RestResponse<LifecycleStageApiResponse>> GetLifecycleStages()
    {
        var request = new RestRequest("table/life_cycle_stage", Method.Get);
        return await _client.ExecuteGetAsync<LifecycleStageApiResponse>(request);
    }

    public async Task<RestResponse<LifecycleStatusesApiResponse>> GetLifecycleStatuses()
    {
        var request = new RestRequest("table/life_cycle_stage_status");
        return await _client.ExecuteGetAsync<LifecycleStatusesApiResponse>(request);
    }

    public async Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetBySerialNumberAsync(string serialNumber)
    {
        var request = new RestRequest("table/alm_hardware", Method.Get);
        request.AddParameter("sysparm_query", $"serial_number={serialNumber}");
        request.AddParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name");
        _logger.LogInformation($"Querying alm_hardware table from asset: {serialNumber}");
        // The following line sends the request and automatically deserializes the response.
        return await _client.ExecuteGetAsync<ServiceNowApiResponse>(request);

    }

    public async Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetByIdAsync(string sys_id)
    {
        var request = new RestRequest("table/alm_hardware", Method.Get);
        request.AddParameter("sysparm_query", $"sys_id={sys_id}");
        request.AddParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name");
        _logger.LogInformation($"Querying database for asset, {sys_id}");
        return await _client.ExecuteGetAsync<ServiceNowApiResponse>(request);
    }

    public async Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetsByIdAsync(string[] sys_ids)
    {
        //Build query string
        string queryString = string.Join("^OR", sys_ids.Select(id => $"sys_id={id}"));
        var request = new RestRequest("table/alm_hardware", Method.Get);
        request.AddParameter("sysparm_query", queryString);
        request.AddParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name");
        _logger.LogInformation($"Querying database for assets: {queryString}");
        return await _client.ExecuteGetAsync<ServiceNowApiResponse>(request);
    }

    public async Task<RestResponse<ServiceNowApiResponse>> RetireServiceNowAssetAsync(string sys_id, object payload)
    {
        var request = new RestRequest($"table/alm_hardware/{sys_id}")
            .AddQueryParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name")
            .AddJsonBody(payload);

        return await _client.ExecutePutAsync<ServiceNowApiResponse>(request);
    }

    public async Task<LifecycleMembers> GetLifecycleMembersAsync()
    {
        if (lifecycleMembers is null)
        {
            var stageRequest = new RestRequest("table/life_cycle_stage", Method.Get);
            stageRequest.AddParameter("sysparm_fields", "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name");
            var statusRequest = new RestRequest("table/life_cycle_stage_status", Method.Get);
            RestResponse<LifecycleStageApiResponse> stageResponse;
            RestResponse<LifecycleStatusesApiResponse> statusResponse;

            try
            {
                stageResponse = await _client.ExecuteGetAsync<LifecycleStageApiResponse>(stageRequest);
                statusResponse = await _client.ExecuteGetAsync<LifecycleStatusesApiResponse>(statusRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(message: "There was an error getting lifecycle members from service now", args: ex);
                return new LifecycleMembers();
            }

            var stages = stageResponse.Data?.LifecycleStages;
            var statuses = statusResponse.Data?.LifecycleStatuses;

            if (stages is not null && statuses  is not null)
            {
                lifecycleMembers = new() { Statuses = statuses, Stages = stages};
                return lifecycleMembers;
            }
        }
        else
        {
            return lifecycleMembers;
        }
        return new LifecycleMembers();
    }
}
