using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DispoDataAssistant.Data.Models
{
    public class AssetTabItem : ObservableObject
    {
        public string? Header { get; set; }
        public IEnumerable<ServiceNowAsset>? Assets { get; set; }
    }
}
