using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace DispoDataAssistant.Data.Models
{
    public partial class TabModel : ObservableObject
    {
        public int Id { get; set; }
        public int? Index { get; set; } = null;
        [ObservableProperty]
        private string name = string.Empty;
        public virtual ObservableCollection<ServiceNowAsset> ServiceNowAssets { get; set; } = new ObservableCollection<ServiceNowAsset>();

        public static TabModel Empty()
        {
            return new TabModel
            {
                Id = 0,
                Name = string.Empty,
                ServiceNowAssets = new ObservableCollection<ServiceNowAsset>()
            };
        }
    }
}
