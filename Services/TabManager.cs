using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Models;
using DispoDataAssistant.Views;
using DispoDataAssistant.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DispoDataAssistant.Services
{
    public partial class TabManager : ObservableObject, ITabManager
    {
        [ObservableProperty]
        private ObservableCollection<AssetTabItem> tabItems;

        [ObservableProperty]
        private int selectedTabIndex;

        [ObservableProperty]
        private string? newTabName;

        [ObservableProperty]
        private string? currentTabName;

        [ObservableProperty]
        private string? deviceId;

        private IWindowService _windowService;
        private TabControlEditViewModel _tabControlEditViewModel;

        public TabManager(IWindowService windowService, TabControlEditViewModel tabControlEditViewModel)
        {
            tabItems = new ObservableCollection<AssetTabItem>();
            _windowService = windowService;
            _tabControlEditViewModel = tabControlEditViewModel;
        }

        public void ClearTabs()
        {
            throw new NotImplementedException();
        }

        public void CombineTabs(List<TabItem> tabItems)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void CreateNewTab()
        {
            _tabControlEditViewModel.NewTabNamePanelVisibility = Visibility.Visible;
            _tabControlEditViewModel.DeviceIdPanelVisibility = Visibility.Visible;
            var vm = _windowService.ShowDialog<TabControlEditWindowView, TabControlEditViewModel>();

            
            //Assign values from view model
            NewTabName = vm.NewTabName;
            DeviceId = vm.DeviceId;
            CurrentTabName = vm.CurrentTabName;

            if(NewTabName is not null)
            {
                DbService.CreateNewTable(NewTabName);
                var tabItem = CreateTabItem(NewTabName);

                if (!TabItems.Any(item => item.Header == NewTabName))
                {
                    TabItems.Insert(0, tabItem);
                    SelectedTabIndex = 0;
                }
            }

            if(DeviceId is not null)
            {

            }
        }

        public void CreateNewTab(string tabName, ServiceNowAsset serviceNowAsset)
        {

        }

        public ObservableCollection<AssetTabItem> CreateTabItems(List<AssetTabItem> items)
        {
            ObservableCollection<AssetTabItem> tabItems = new ObservableCollection<AssetTabItem>();

            foreach (AssetTabItem item in items)
            {
                if (!TabItems.Contains(item))
                {
                    tabItems.Add(item);
                }
            }
            return tabItems;
        }

        public AssetTabItem CreateTabItem(string tabName)
        {
            return new AssetTabItem
            {
                Header = tabName,
            };
        }

        public AssetTabItem CreateTabItem(string tabName, IEnumerable<ServiceNowAsset> serviceNowAssets)
        {
            return new AssetTabItem
            {
                Header = tabName,
                Assets = serviceNowAssets
            };
        }

        public void RemoveTab(string tabName)
        {
            throw new NotImplementedException();
        }

        public void RenameTab(string oldTabName, string newTabName)
        {
            throw new NotImplementedException();
        }

        public void SaveTabs(List<TabItem> tabItems)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private async Task TabControlLoadedAsync()
        {
            List<AssetTabItem> tabItems = new List<AssetTabItem>();
            List<string>? tableNames = DbService.GetTableNames();

            if (tableNames is not null)
            {
                foreach (string tableName in tableNames)
                {
                    IEnumerable<ServiceNowAsset> table = await DbService.LoadServiceNowAssetsByTable(tableName);

                    AssetTabItem tabItem = CreateTabItem(tableName, table);
                    tabItems.Add(tabItem);
                }
                tabItems.Reverse();
                TabItems = CreateTabItems(tabItems);
                SelectedTabIndex = 0;
            }
        }
    }
}
