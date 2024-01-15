using DispoDataAssistant.Data.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DispoDataAssistant.Factories
{
    public class TabItemFactory
    {
        public static AssetTabItem CreateTabItem(string name, IEnumerable<ServiceNowAsset> table)
        {
            return new AssetTabItem
            {
                Header = name,
                Assets = table,
            };
        }

        public static ObservableCollection<AssetTabItem> CreateTabItems(List<AssetTabItem> items)
        {
            ObservableCollection<AssetTabItem> tabItems = [.. items];
            return tabItems;
        }
    }
}
