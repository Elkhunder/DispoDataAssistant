using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using DispoDataAssistant.UIComponents.Dialogs.AdvancedQuery;
using RestSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces;

public interface IServiceNowApiClient
{
    Task<ServiceNowAsset> RetireServiceNowAssetAsync(string sys_id, RetireDevicePayload payload);
    Task<LifecycleMembers> GetLifecycleMembersAsync();
    Task<IEnumerable<ServiceNowAsset>> GetServiceNowAssetsAsync(List<string> deviceIds);
    Task<IEnumerable<ServiceNowAsset>> GetServiceNowAssetsAsync(List<string> deviceIds, string idType);
    Task<ServiceNowAsset> GetServiceNowAssetAsync(string deviceId);
    Task<ServiceNowAsset> GetServiceNowAssetAsync(string deviceId, string idType);
    Task<IEnumerable<ServiceNowAsset>> GetAssetsByQueryAsync(ObservableCollection<QueryViewModel> queries);
}
