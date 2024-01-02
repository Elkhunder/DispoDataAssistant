using DispoDataAssistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DispoDataAssistant.Services
{
    public interface ITabManager
    {
        void RenameTab(string oldTabName, string newTabName);
        void RemoveTab();
        void ClearTabs();
        void CombineTabs(List<TabItem> tabItems);
        ObservableCollection<AssetTabItem> CreateTabItems(List<AssetTabItem> tabItems);
        AssetTabItem CreateTabItem(string tabName, IEnumerable<ServiceNowAsset> serviceNowAssets);
        void SaveTabs(List<TabItem> tabItems);
    }
}
