using DispoDataAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces
{
    public interface IServiceNowApiClient
    {
        Task<ServiceNowAsset?> GetServiceNowAssetAsync(string assetTag);
    }
}
