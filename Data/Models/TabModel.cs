using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DispoDataAssistant.Data.Models
{
    public partial class TabModel : ObservableObject
    {
        public int Id { get; set; }
        public int? Index { get; set; } = null;
        [ObservableProperty]
        private string name = string.Empty;
        public virtual ObservableCollection<ServiceNowAsset> ServiceNowAssets { get; set; } = new ObservableCollection<ServiceNowAsset>();
    }
}
