using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DispoDataAssistant.Data.Models
{
    public partial class TabModel : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        private string name;
        public virtual ObservableCollection<ServiceNowAsset>? ServiceNowAssets { get; set; }
    }
}
