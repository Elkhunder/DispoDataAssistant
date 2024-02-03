using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using RestSharp;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces;

public interface IServiceNowApiClient
{
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetByAssetTagAsync(string assetTag);
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetBySerialNumberAsync(string serialNumber);
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetsByIdAsync(string[] sys_ids);
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetByIdAsync(string sys_id);
    Task<RestResponse<RetireAssetApiResponse>> RetireServiceNowAssetAsync(string sys_id, object payload);
    Task<LifecycleMembers> GetLifecycleMembersAsync();
}
