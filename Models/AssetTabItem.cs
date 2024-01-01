using CommunityToolkit.Mvvm.ComponentModel;
using DispoDataAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DispoDataAssistant.Models
{
    public class AssetTabItem : ObservableObject
    {
        public string? Header { get; set; }
        public IEnumerable<ServiceNowAsset>? Assets { get; set; }
    }
}
