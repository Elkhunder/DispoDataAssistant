using DispoDataAssistant.Data.Enums;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Services.Implementations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace DispoDataAssistant.Services;

public class ServiceNowApiClient : BaseService, IServiceNowApiClient
{
    private readonly HttpClient _client = new();
    private LifecycleMembers? lifecycleMembers;
    private readonly string _baseUrl = "https://ummeddev.service-now.com/api/now";

    public ServiceNowApiClient(string baseUrl, ILogger logger) : base(logger)
    {
        // Define file paths and filenames for encryption key and initialization vector (IV)

        //string sharedDrivePath = @"\\corefs.med.umich.edu\Shared2\MCIT_Shared\Teams\DES_ALL\DispoDataAssistant\ServiceNow";
        string sharedDrivePath = @"C:\Users\jsissom\ServiceNow";
        string encryptionKeyFileName = "EncryptionKey.txt";
        string ivFileName = "InitializationVector.txt";

        EncryptionService encryptionService = new EncryptionService(sharedDrivePath, encryptionKeyFileName, ivFileName);
        var (serviceNowUsername, serviceNowPassword) = encryptionService.GetDecryptedServiceNowCredentials("ServiceNowUsername", "ServiceNowPassword");

        if (serviceNowUsername is not null && serviceNowPassword is not null)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{serviceNowUsername}:{serviceNowPassword}");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }

    public async Task<IEnumerable<ServiceNowAsset>> GetServiceNowAssetsAsync(List<string> deviceIds)
    {
        var response = await GetServiceNowAssetsByDeviceIdsAsync(deviceIds);
        
        return await ProcessServiceNowAssetsAsync(response);
    }

    private async Task<HttpResponseMessage> GetServiceNowAssetsByDeviceIdsAsync(List<string> deviceIds)
    {
        try
        {
            _logger.LogInformation($"Querying database for assets, {deviceIds}");

            var request = new HttpRequestMessage(HttpMethod.Get, QueryStringBuilder(deviceIds));
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch(HttpRequestException ex)
        {
            if(ex.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                //potentially set timer and retry a few times
                throw;
            }
            else
            {
                _logger.LogError($"HTTP Request Exception received, Status Code:{ex.StatusCode}, Message:{ex.Message}, Stack Trace:{ex.StackTrace}");
                throw;
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Message:{ex.Message}, Stack Trace:{ex.StackTrace}");
            throw;
        }
    }

    private string QueryStringBuilder(List<string> deviceIds)
    {
        var builder = new UriBuilder($"{_baseUrl}/table/alm_hardware");
        var query = HttpUtility.ParseQueryString(builder.Query);
        var queryBy = new List<string>();
        
        foreach ( var deviceId in deviceIds )
        {
            switch (deviceId)
            {
                case string d when string.IsNullOrEmpty(d):
                    _logger.LogError($"Device ID: {deviceId} is null or empty");
                    break;
                case string d when Int32.TryParse(d, out var _):
                    queryBy.Add($"{deviceIdTypeMap[DeviceIdType.AssetTag]}={deviceId}");
                    break;
                case string d when d.Length >= 7 && d.Length <= 10 && d.All(c => char.IsLetterOrDigit(c)):
                    queryBy.Add($"{deviceIdTypeMap[DeviceIdType.SerialNumber]}={deviceId}");
                    break;
                case string d when Enum.GetNames(typeof(DeviceNamePrefix)).Any(prefix => d.StartsWith(prefix)):
                    queryBy.Add($"{deviceIdTypeMap[DeviceIdType.DeviceName]}={deviceId}");
                    break;
                case string d when deviceId.Length == 32 && deviceId.All(c => (c >= '0' && c <= '9') ||
                                                                              (c >= 'a' && c <= 'f') ||
                                                                              (c >= 'A' && c <= 'F')):
                    //Device id is a sys_id
                    queryBy.Add($"{deviceIdTypeMap[DeviceIdType.SysId]}={deviceId}");
                    break;
                default:
                    //Invalid device id format
                    _logger.LogError($"Device ID: {deviceId} is an {DeviceIdType.Invalid} format");
                    break;
            }
        }
        query["sysparm_query"] = string.Join(" ^ OR ", queryBy);
        query["sysparm_fields"] = "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name";
        builder.Query = query.ToString();

        return builder.ToString();
    }

    private async Task<IEnumerable<ServiceNowAsset>> ProcessServiceNowAssetsAsync(HttpResponseMessage response)
    {
        try
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var serviceNowApiResponse = JsonSerializer.Deserialize<ServiceNowApiResponse>(content);
            
            if (serviceNowApiResponse is null)
            {
                _logger.LogError($"Service now api response object was null");
                return Enumerable.Empty<ServiceNowAsset>();
            }
            
            return serviceNowApiResponse.Assets;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Enumerable.Empty<ServiceNowAsset>();
        }
    }

    public async Task<ServiceNowAsset> RetireServiceNowAssetAsync(string sys_id, RetireDevicePayload payload)
    {
        HttpResponseMessage retireResponse;
        if (payload == null)
        {
            _logger.LogError($"{sys_id}: paylod was null");
            return ServiceNowAsset.Empty();
        }
        try
        {
            var builder = new UriBuilder($"{_baseUrl}/table/alm_hardware/{sys_id}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["sysparm_fields"] = "sys_id,parent,asset_tag,model.name,model_category.name,sys_updated_on,substatus,install_status,life_cycle_stage.name,life_cycle_stage_status.name,serial_number,model.manufacturer.name";
            builder.Query = query.ToString();
            var url = builder.ToString();

            retireResponse = await _client.PutAsJsonAsync<RetireDevicePayload>(url, payload);
            retireResponse.EnsureSuccessStatusCode();

            var content = await retireResponse.Content.ReadAsStringAsync();
            var asset = JsonSerializer.Deserialize<RetireAssetApiResponse>(content)?.ServiceNowAsset;
            if (asset is null)
            {
                _logger.LogError("Asset was null following serialization");
                return ServiceNowAsset.Empty();
            }
            return asset;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
            return ServiceNowAsset.Empty();
        }
    }

    public async Task<LifecycleMembers> GetLifecycleMembersAsync()
    {
        if (lifecycleMembers is null)
        {
            HttpResponseMessage stageResponse;
            HttpResponseMessage statusResponse;

            lifecycleMembers = new();
            try
            {
                stageResponse = await _client.GetAsync($"{_baseUrl}/table/life_cycle_stage");
                statusResponse = await _client.GetAsync($"{_baseUrl}/table/life_cycle_stage_status");

                if (!stageResponse.IsSuccessStatusCode || !statusResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("There was an error getting lifecycle members from service now. Stage response status: {0}, Status response status: {1}", stageResponse.StatusCode, statusResponse.StatusCode);
                    return LifecycleMembers.Empty();
                }

                var stageResponseContent = await stageResponse.Content.ReadAsStringAsync();
                var statusResponseContent = await statusResponse.Content.ReadAsStringAsync();


                var stages = JsonSerializer.Deserialize<LifecycleStageApiResponse>(stageResponseContent)?.LifecycleStages;
                var statuses = JsonSerializer.Deserialize<LifecycleStatusesApiResponse>(statusResponseContent)?.LifecycleStatuses;

                if (stages is not null && statuses is not null)
                {
                    lifecycleMembers = new() { Statuses = statuses, Stages = stages };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"There was an error getting lifecycle members from service now {ex.Message}");
                return LifecycleMembers.Empty();
            }
        }
        return lifecycleMembers;
    }

    readonly Dictionary<DeviceIdType, string> deviceIdTypeMap = new()
    {
        { DeviceIdType.AssetTag, "asset_tag" },
        { DeviceIdType.SerialNumber, "serial_number" },
        { DeviceIdType.SysId, "sys_id" },
        { DeviceIdType.DeviceName, "device_name" },
        { DeviceIdType.Invalid, "invalid" }
    };
}
