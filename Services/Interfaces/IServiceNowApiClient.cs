using DispoDataAssistant.Data.Models;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces;

public interface IServiceNowApiClient
{
    Task<ServiceNowAsset?> GetServiceNowAssetByAssetTagAsync(string assetTag);
    Task<ServiceNowAsset?> GetServiceNowAssetBySerialNumberAsync(string serialNumber);
    Task<ServiceNowAsset?> RetireServiceNowAssetAsync(string sys_id);
}
