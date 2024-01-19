using DispoDataAssistant.Data.Models;
using System.Collections.Generic;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface ITabService
    {
        IEnumerable<AssetTabItem> GetAllTabs();
        void RenameTab(AssetTabItem tab, string newName);
    }
}
