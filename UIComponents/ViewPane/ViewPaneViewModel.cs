using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Factories;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Implementations;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DispoDataAssistant.UIComponents.ViewPane
{
    public partial class ViewPaneViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ServiceNowAsset> selectedAssets = [];
        [ObservableProperty]
        private ObservableCollection<ServiceNowAsset> assetsCollection;
        [ObservableProperty]
        private ObservableCollection<AssetTabItem> tabItems = [];
        [ObservableProperty]
        private int selectedTabIndex;
        [ObservableProperty]
        private AssetTabItem? selectedTab;
        [ObservableProperty]
        private string? currentTabName;
        [ObservableProperty]
        private string? newTabName;
        [ObservableProperty]
        private string? deviceId;

        private readonly TabItemFactory _tabItemFactory;
        private readonly ServiceNowAssetContext _context;

        public ViewPaneViewModel(ILogger<ViewPaneViewModel> logger, TabItemFactory tabItemFactory, ServiceNowAssetContext context) : base(logger, null!)
        {
            _tabItemFactory = tabItemFactory;
            _context = context;
            _messenger.Register<RefreshTabsMessage>(this, OnRefreshTabsMessageReceived);
            _messenger.Register<RequestTabMessage>(this, OnRequestedTabMessageReceived);

            _context.ServiceNowAssets.Load();
            AssetsCollection = _context.ServiceNowAssets.Local.ToObservableCollection();
        }

        private void OnRequestedTabMessageReceived(object recipient, RequestTabMessage message)
        {
            message.Reply(selectedTab);
        }

        private async void OnRefreshTabsMessageReceived(object recipient, RefreshTabsMessage message)
        {
            //await TabControlLoadedAsync(null!);
        }

        [RelayCommand]
        private static void SyncSelectedAssets(IEnumerable<ServiceNowAsset> collection)
        {

        }

        [RelayCommand]
        private async Task DataGridLoadedAsync(System.Windows.Controls.DataGrid dataGrid)
        {


            List<AssetTabItem> items = [];
            List<string>? tableNames = DbService.GetTableNames();

            if (tableNames is not null)
            {
                foreach (string tableName in tableNames)
                {
                    IEnumerable<ServiceNowAsset> table = await DbService.LoadServiceNowAssetsByTable(tableName);

                    AssetTabItem item = TabItemFactory.CreateTabItem(tableName, table);
                    items.Add(item);
                }
                items.Reverse();
                TabItems.Clear();
                TabItems = TabItemFactory.CreateTabItems(items);
                SelectedTabIndex = 0;
            }
        }
    }
}
