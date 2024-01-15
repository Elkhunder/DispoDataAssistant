using DispoDataAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface ITabService
    {
        IEnumerable<AssetTabItem> GetAllTabs();
        void RenameTab(AssetTabItem tab, string newName);
    }
}
