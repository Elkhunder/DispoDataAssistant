using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Implementations;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DispoDataAssistant.UIComponents
{
    //Create new view for tab control buttons and move some of the tab functionality from the viewpane view model to here.  Specifically stuff that has to do with manipulating tabs.
    public partial class TabControlButtonsViewModel : BaseViewModel
    {
        private ServiceNowAssetContext _context;

        public TabControlButtonsViewModel() : base(null!, null!) { }
        public TabControlButtonsViewModel(ILogger<TabControlButtonsViewModel> logger, IWindowService windowService, ServiceNowAssetContext context) : base(logger, windowService)
        {
            _context = context;
        }

        public void ClearTabs()
        {
            throw new NotImplementedException();
        }

        public void CombineTabs(AssetTabItem assetTabItem)
        {
            _messenger.Send(new CombineTabsMessage(assetTabItem));
        }


        public void CreateNewTab(int? selectedTabIndex)
        {
            
            var vm = _windowService.ShowDialog<TabControlEditWindowView, TabControlEditViewModel>();

            if (vm.NewTabName is not null)
            {
                DbService.CreateNewTable(vm.NewTabName);
                //Create messenger to send a request to have the tabs resynced
                _messenger.Send<RefreshTabsMessage>(new RefreshTabsMessage());
            }

            //if (deviceId is not null)
            //{
            //    //Serial number will have letters, asset tag will be all numbers.
            //    //check if device id can be converted to an integer, if not then it's a serial number.
            //    if (int.TryParse(deviceId, out var id))
            //    {
            //        //Search for asset in service now by it's asset tag
            //    }
            //    else
            //    {
            //        //Search for asset in service by by it's serial number
            //    }
            //}
            _messenger.Send<RefreshTabsMessage>(new RefreshTabsMessage());
        }

        [RelayCommand]
        public void RemoveTab(AssetTabItem selectedTab)
        {
            if (selectedTab.Header is not null)
            {
                _logger.LogInformation($"{selectedTab.Header} was removed by user");
                DbService.DropTable(selectedTab.Header);
                WeakReferenceMessenger.Default.Send<RefreshTabsMessage>(new RefreshTabsMessage());
            }
        }

        [RelayCommand]
        public async void SaveChanges(DataGrid dataGrid)
        {
            await _context.SaveChangesAsync();
        }

        [RelayCommand]
        public void RenameTab()
        {
            var selectedTab = _messenger.Send<RequestTabMessage>().Response;
            _messenger.Send(new RenameTabMessage(selectedTab));
            //_messenger.Send<RefreshTabsMessage>(new RefreshTabsMessage());
        }

        public void SaveTabs(List<TabItem> tabItems)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private static void DownloadFile()
        {
            //string? pickupDate = _tabControlEditViewModel.PickupDate;
            //string? pickupLocation = _tabControlEditViewModel.PickupLocation;

            //if (pickupDate is null)
            //{
            //    pickupDate = DateTime.Now.ToString();
            //}

            //if (pickupLocation is null)
            //{
            //    pickupLocation = "ArborLakes";
            //}

            //string fileName = $"{pickupLocation}-Dispo-{pickupDate}";
            //try
            //{

            //}
            //catch
            //{

            //}
        }
    }
}
