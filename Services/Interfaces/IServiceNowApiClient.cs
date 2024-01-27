using DispoDataAssistant.Data.Models;
using RestSharp;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces;

public interface IServiceNowApiClient
{
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetByAssetTagAsync(string assetTag);
    Task<RestResponse<ServiceNowApiResponse>> GetServiceNowAssetBySerialNumberAsync(string serialNumber);
    Task<RestResponse<ServiceNowApiResponse>> RetireServiceNowAssetAsync(string sys_id);
}
