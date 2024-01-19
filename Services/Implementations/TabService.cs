using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Services.Implementations
{
    public class TabService : BaseService, ITabService
    {
        public TabService(ILogger<TabService> logger) : base(logger)
        {
        }

        public IEnumerable<AssetTabItem> GetAllTabs()
        {
            throw new NotImplementedException();
        }

        public void RenameTab(AssetTabItem tab, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
