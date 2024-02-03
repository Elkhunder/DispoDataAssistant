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
                   existingAsset.Substate != newAsset.Substate ||
                   existingAsset.State != newAsset.State ||
                   existingAsset.LastUpdated != newAsset.LastUpdated ||
                   existingAsset.LifeCycleStatus != newAsset.LifeCycleStatus ||
                   existingAsset.LifeCycleStage != newAsset.LifeCycleStage ||
                   existingAsset.Parent != newAsset.Parent;

        }
    }
}
