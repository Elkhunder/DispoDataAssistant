using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces;

public interface IServiceNowApiClient
{
    Task<ServiceNowAsset> RetireServiceNowAssetAsync(string sys_id, RetireDevicePayload payload);
    Task<LifecycleMembers> GetLifecycleMembersAsync();
    Task<IEnumerable<ServiceNowAsset>> GetServiceNowAssetsAsync(List<string> deviceIds);
}
