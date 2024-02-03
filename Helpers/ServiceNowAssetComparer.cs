using DispoDataAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Helpers
{
    public class ServiceNowAssetComparer
    {
        public static bool IsDifferent(ServiceNowAsset existingAsset, ServiceNowAsset newAsset)
        {
            return existingAsset.AssetTag != newAsset.AssetTag ||
                   existingAsset.Manufacturer != newAsset.Manufacturer ||
                   existingAsset.Model != newAsset.Model ||
                   existingAsset.Category != newAsset.Category ||
                   existingAsset.SerialNumber != newAsset.SerialNumber ||
                   existingAsset.OperationalStatus != newAsset.OperationalStatus ||
                   existingAsset.InstallStatus != newAsset.InstallStatus ||
                   existingAsset.LastUpdated != newAsset.LastUpdated;
        }
    }
}
