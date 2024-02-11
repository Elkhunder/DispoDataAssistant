using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CsvHelper;
using CsvHelper.Configuration;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Enums;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.ServiceNow;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Helpers;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using DispoDataAssistant.UIComponents.Dialogs;
using GongSolutions.Wpf.DragDrop;
using MaterialDesignThemes.MahApps;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DispoDataAssistant.UIComponents.Main;

public partial class MainViewModel : BaseViewModel, IDropTarget
{
    // Binding properties
    [ObservableProperty]
    private ObservableCollection<TabModel> tabs = [];
    [ObservableProperty]
    private TabModel selectedTab = new();
    [ObservableProperty]
    private int selectedTabIndex;
    [ObservableProperty]
    private ServiceNowAsset? selectedAsset = new();
    [ObservableProperty]
    private ObservableCollection<ServiceNowAsset> selectedAssets = new();
    [ObservableProperty]
    private string serialNumber = string.Empty;
    [ObservableProperty]
    private string deviceId = string.Empty;
    [ObservableProperty]
    private string searchBy = string.Empty;
    [ObservableProperty]
    private List<DeviceIdType> deviceIdTypes = Enum.GetValues(typeof(DeviceIdType)).Cast<DeviceIdType>().Where(v => v != DeviceIdType.Invalid).ToList();

    // DI Injections
    private AssetContext _assetContext;
    private TabControlEditWindowView _tabControlEditWindow;
    private IServiceNowApiClient _serviceNowApiClient;
    private IFileDialogService _fileDialogService;

    public MainViewModel() : this(null!, null!, null!, null!, null!,null!) { }

    public MainViewModel(ILogger<MainViewModel> logger, AssetContext assetContext, TabControlEditWindowView tabControlEditWindow, IServiceNowApiClient serviceNowApiClient, IFileDialogService fileDialogService, ISnackbarMessageQueue messageQueue) : base(logger, null!,messageQueue)
    {
        Console.WriteLine("MainViewModel: Instance Created");
        _assetContext = assetContext;
        _tabControlEditWindow = tabControlEditWindow;
        _serviceNowApiClient = serviceNowApiClient;
        _fileDialogService = fileDialogService;
    }

    // Event Methods
    [RelayCommand]
    private void OnMainWindow_OnClick(MouseButtonEventArgs e)
    {
        if (SelectedAssets.Count > 1)
        {
            SelectedAsset = null;
        }
    }
    [RelayCommand]
    private void OnMainWindow_Loaded()
    {
        if (_assetContext.Tabs.Any())
        {
            _assetContext.Tabs.OrderBy(tab => tab.Index).Load();
            var tabs = _assetContext.Tabs.Local;

            bool isIndexUpdated = false;

            if (tabs != null && tabs.Count > 0)
            {
                int index = 0;
                foreach (var tab in tabs)
                {
                    if (tab.Index is null)
                    {
                        isIndexUpdated = true;
                        tab.Index = index;
                        index++;
                    }
                }
                if (isIndexUpdated)
                {
                    _assetContext.SaveChanges();
                }

                Tabs = _assetContext.Tabs.Local.ToObservableCollection();
            }
        }
    }
    [RelayCommand]
    private void OnChangeTheme()
    {
        var paletteHelper = new PaletteHelper();
        ITheme currentTheme = paletteHelper.GetTheme();
        if (currentTheme.GetBaseTheme() is BaseTheme.Dark)
        {
            currentTheme.SetBaseTheme(Theme.Light);
        }
        else
        {
            currentTheme.SetBaseTheme(Theme.Dark);
        }
        paletteHelper.SetTheme(currentTheme);

        MessageQueue.Enqueue($"Theme changed to {currentTheme.GetBaseTheme()}");
    }

    [RelayCommand]
    private async Task OnEnterPressed(KeyEventArgs e)
    {
        if(e.Key is Key.Enter)
        {
            await OnQueryServiceNow();
        }
    }

    [RelayCommand]
    private async Task OnRetireAssetFromServiceNow()
    {
        var assets = new ObservableCollection<ServiceNowAsset>();
        if (SelectedAssets is not null && SelectedAssets.Count > 0)
        {
            _logger.LogInformation("Assets collection is not null");
            assets = SelectedAssets;
            //Check to see what selected assets are not already retired prior to sending request to service now
        }
        else
        {
            _logger.LogError("Assets collection is null, getting all assets from selected tab");
            assets = SelectedTab.ServiceNowAssets;
            //Find out what assets are not already retired
            //Send request to service now to retire assets
        }
        var lifecycleMembers = await _serviceNowApiClient.GetLifecycleMembersAsync();
        string? installStatus = ((int)ServiceNowInstallStatus.Retired).ToString();
        string? substatus = lifecycleMembers.Statuses?.SingleOrDefault(s => s.Name == "Disposed")?.Name ?? "Disposed";
        string? state = lifecycleMembers.Statuses?.SingleOrDefault(s => s.Name == "Retired")?.Name ?? "Retired";
        string? lifecycleStage = lifecycleMembers.Stages?.SingleOrDefault(s => s.Name == "End of Life")?.Name ?? "End of Life";
        string? lifecycleStatus = lifecycleMembers.Statuses?.SingleOrDefault(s => s.Name == "Disposed")?.Name ?? "Disposed";

        var payload = new RetireDevicePayload
        {
            InstallStatus = installStatus,
            Substatus = substatus,
            LifecycleStage = lifecycleStage,
            LifecycleStatus = lifecycleStatus
        };

        var tasks = assets.Select(async asset =>
        {
            if (asset.IsRetired(payload))
            {
                return;
            }
            if (!string.IsNullOrEmpty(asset.Parent))
            {
                payload.Parent = string.Empty;
            }
            if (asset.SysId is not null)
            {
                var updatedAsset = await _serviceNowApiClient.RetireServiceNowAssetAsync(asset.SysId, payload);

                if (updatedAsset is not null)
                {

                    asset.State = updatedAsset.State;
                    asset.Substate = updatedAsset.Substate;
                    asset.LifeCycleStage = updatedAsset.LifeCycleStage;
                    asset.LifeCycleStatus = updatedAsset.LifeCycleStatus;
                    asset.Parent = updatedAsset.Parent;
                    asset.LastUpdated = updatedAsset.LastUpdated;
                }
            }
        });

        await Task.WhenAll(tasks);

        _assetContext.Tabs.Update(SelectedTab);
        await _assetContext.SaveChangesAsync();
    }

    // Service Now Query Methods
    [RelayCommand]
    private async Task OnSyncWithServiceNow()
    {
        List<string>? sysIds = SelectedTab.ServiceNowAssets.Select(asset => asset.SysId).ToList()!;
        if (sysIds is not null && sysIds.Count > 0)
        {
            var assets = await _serviceNowApiClient.GetServiceNowAssetsAsync(sysIds);
            foreach (var asset in assets)
            {
                if (string.IsNullOrEmpty(asset.SysId)) continue;

                var existingAsset = SelectedTab.ServiceNowAssets.First(a => a.SysId == asset.SysId);

                if (ServiceNowAssetComparer.IsDifferent(existingAsset, asset))
                {
                    existingAsset.Manufacturer = asset.Manufacturer;
                    existingAsset.SerialNumber = asset.SerialNumber;
                    existingAsset.LastUpdated = asset.LastUpdated;
                    existingAsset.State = asset.State;
                    existingAsset.AssetTag = asset.AssetTag;
                    existingAsset.Category = asset.Category;
                    existingAsset.Model = asset.Model;
                    existingAsset.Substate = asset.Substate;
                    existingAsset.LifeCycleStage = asset.LifeCycleStage;
                    existingAsset.LifeCycleStatus = asset.LifeCycleStatus;
                    existingAsset.Parent = asset.Parent;
                }
            }
            _assetContext.Tabs.Update(SelectedTab);
        }
    }
    [RelayCommand]
    private async Task OnQueryServiceNow()
    {
        var vm = new NewTabViewModel();
        var dialogResult = await DialogHost.Show(vm, "TabNameDialog");

        if(dialogResult is bool boolResult && boolResult && vm.IsTabNameToggled)
        {
            var tab = new TabModel()
            {
                Name = vm.Name,
                ServiceNowAssets = []
            };
            _assetContext.Tabs.Local.Add(tab);
            _assetContext.SaveChanges();
            SelectedTab = tab;
        }
        //Device ID not being set when hitting enter 
        
        //var tab = SendNewTabRequest() ?? SelectedTab;
        //tab.ServiceNowAssets ??= [];
        if (string.IsNullOrEmpty(DeviceId))
        {
            _logger.LogError("Device Id was null");
            MessageQueue.Enqueue("Device Id can't be empty");
            //MessageBox.Show("Device ID can not be empty");
            return;
        }
        if (SelectedTab.ServiceNowAssets.Any(asset => asset.SerialNumber == DeviceId))
        {
            MessageBox.Show($"Tab already contains asset with the provided serial number: {DeviceId}");
            return;
        }
        if (SelectedTab.ServiceNowAssets.Any(asset => asset.AssetTag == DeviceId))
        {
            MessageBox.Show($"Tab already contains asset with the provided asset tag: {DeviceId}");
            return;
        }

        List<string> deviceIds = [.. DeviceId.Split(',')];

        if (!string.IsNullOrEmpty(SearchBy) && deviceIds.Count is 1)
        {
            var deviceId = deviceIds.FirstOrDefault(string.Empty);
            var asset = await _serviceNowApiClient.GetServiceNowAssetAsync(deviceId, SearchBy);

            SelectedTab.ServiceNowAssets.Add(asset);
            _assetContext.SaveChanges();
            //Call a getservicenowassetasync method and search for a single asset one the search by value provided.
        }
        else if (!string.IsNullOrEmpty(SearchBy) && deviceIds.Count > 1)
        {
            var assets = await _serviceNowApiClient.GetServiceNowAssetsAsync(deviceIds, SearchBy);
            foreach (var asset in assets)
            {
                SelectedTab.ServiceNowAssets.Add(asset);
            }
            _assetContext.SaveChanges();
        }
        else if (deviceIds.Count is 1)
        {
            var deviceId = deviceIds.FirstOrDefault(string.Empty);

            var asset = await _serviceNowApiClient.GetServiceNowAssetAsync(deviceId);
            SelectedTab.ServiceNowAssets.Add(asset);
            _assetContext.SaveChanges();
            //Call a get service now asset async method and search for a single asset based on the infered device id type
        }
        else if (deviceIds.Count > 1)
        {
            //if there is more than one id in device ids call the get service now assets async method and search for all assets based on the infered device type ids
            var assets = await _serviceNowApiClient.GetServiceNowAssetsAsync(deviceIds);

            foreach (var asset in assets)
            {
                SelectedTab.ServiceNowAssets.Add(asset);
            }
            _assetContext.SaveChanges();
        }
    }

    // Tab Action Methods
    [RelayCommand]
    private async Task OnCreateTab()
    {
        var vm = new NewTabViewModel();
        vm.IsTabNameToggled = true;
        vm.ToggleVisibility = Visibility.Collapsed;
        vm.Title = "Enter New Tab Name";
        var dialogResult = await DialogHost.Show(vm, "TabNameDialog");

        if (dialogResult is bool boolResult && boolResult)
        {
            var tab = new TabModel()
            {
                Name = vm.Name,
                ServiceNowAssets = []
            };
            _assetContext.Tabs.Local.Add(tab);
            _assetContext.SaveChanges();
            SelectedTab = tab;
        }
        //if (SendNewTabRequest() is null)
        //{
        //    MessageBox.Show("New tab was unable to be created.");
        //}
    }

    private TabModel? SendNewTabRequest()
    {
        var newTabNameRequest = new RequestNewTabNameMessage();
        _messenger.Send(newTabNameRequest);
        if (newTabNameRequest.HasReceivedResponse)
        {
            string name = newTabNameRequest.Response;
            int index = _assetContext.Tabs.Count() - 1;
            var tab = new TabModel() { Index = index, Name = name, ServiceNowAssets = [] };
            _assetContext.Tabs.Add(tab);
            _assetContext.SaveChanges();

            return tab;
        }
        return null;
    }

    [RelayCommand]
    private void OnDeleteTab(EventArgs e)
    {
         _assetContext.Tabs.Remove(SelectedTab);
         _assetContext.SaveChanges();
        
    }


    [RelayCommand]
    private async Task OnRenameTab()
    {
        //tab name is not updating in real time
        var tab = SelectedTab;

        var vm = new NewTabViewModel();
        vm.IsTabNameToggled = true;
        vm.ToggleVisibility = Visibility.Collapsed;
        vm.Title = "Enter New Tab Name";
        var dialogResult = await DialogHost.Show(vm, "TabNameDialog");

        if (dialogResult is bool boolResult && boolResult)
        {
            SelectedTab.Name = vm.Name;
        }

        //var newTabNameRequest = _messenger.Send(new RequestNewTabNameMessage());


        //if (newTabNameRequest.HasReceivedResponse)
        //{
        //    var newTabName = newTabNameRequest.Response;
        //    if (string.IsNullOrEmpty(newTabName))
        //    {
        //        MessageBox.Show("New Tab Name was empty when attempting to change tab name");
        //        return;
        //    }
        //    else if (SelectedTab.Name == newTabName)
        //    {
        //        MessageBox.Show("New tab name was the same as the current tab name");
        //        return;
        //    }
        //    SelectedTab.Name = newTabName;
        //    _assetContext.Tabs.Update(SelectedTab);
        //}
        _assetContext.SaveChanges();
    }

    [RelayCommand]
    private async Task OnUploadAssets()
    {
        var vm = new NewTabViewModel();
        var dialogResult = await DialogHost.Show(vm, "TabNameDialog");

        if (dialogResult is bool boolResult && boolResult)
        {
            var tab = new TabModel()
            {
                Name = vm.Name,
                ServiceNowAssets = []
            };
            _assetContext.Tabs.Local.Add(tab);
            _assetContext.SaveChanges();
            SelectedTab = tab;
        }
        OpenFileDialog openFileDialog = new();

        bool? result = openFileDialog.ShowDialog();

        if (result == false)
        {
            _logger.LogInformation("Operation canceled by user");
            MessageQueue.Enqueue("Operation Cancelled");
            return;
        }
        var file = openFileDialog.FileName;
        if (string.IsNullOrEmpty(file))
        {
            _logger.LogError("User did not provide a file to the dialog box");
            MessageQueue.Enqueue("File Not Provided");
            return;
        }
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        };
        using (var sr = new StreamReader(file))
        using (var csvReader = new CsvReader(sr, config))
        {
            var records = csvReader.GetRecords<ServiceNowAsset>();
            var Ids = new string[records.Count()];
            var deviceIds = new List<string>();
            foreach (var record in records)
            {
                if (record.IsComplete())
                {
                    record.Tab = SelectedTab;
                    record.TabId = SelectedTab.Id;
                    SelectedTab.ServiceNowAssets?.Add(record);
                    continue;
                }
                if (!string.IsNullOrEmpty(record.SysId))
                {
                    deviceIds.Add(record.SysId);
                    continue;
                }
                if (!string.IsNullOrEmpty(record.SerialNumber))
                {
                    deviceIds.Add(record.SerialNumber);
                    continue;
                }
                if (!string.IsNullOrEmpty(record.AssetTag))
                {
                    deviceIds.Add(record.AssetTag);
                    continue;
                }
                //else if (!string.IsNullOrEmpty(record.Name))
                //{
                //    deviceIds.Add(record.Name);
                //    continue;
                //}
            }
            var assets = await _serviceNowApiClient.GetServiceNowAssetsAsync(deviceIds);

            foreach (var asset in assets)
            {
                if (asset is null)
                {
                    _logger.LogError($"Asset was not found in Service Now");
                    continue;
                }
                SelectedTab.ServiceNowAssets?.Add(asset);
            }
        }
        _assetContext.SaveChanges();
    }

    [RelayCommand]
    private void OnDownloadAssets()
    {
        var file = _fileDialogService.SaveFileDialog();
        var tab = SelectedTab;

        var assets = tab.ServiceNowAssets;

        using (StreamWriter writer = new(new FileStream(file, FileMode.Create, FileAccess.Write)))
        {
            CsvHandler csvHandler = new();
            string delimeter = "\t";
            writer.Write(csvHandler.BuildCsvHeader(delimeter));

            foreach (var asset in assets)
            {
                string line = csvHandler.ConvertAssetToCsvLine(asset, delimeter);
                writer.WriteLine(line);
            }
        }
    }

    void IDropTarget.DragOver(IDropInfo dropInfo)
    {
        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        dropInfo.Effects = DragDropEffects.Move;
    }

    void IDropTarget.Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is TabModel sourceItem)
        {
            var sourceIndex = Tabs.IndexOf(sourceItem);
            var targetIndex = dropInfo.InsertIndex;

            // Ensure targetIndex is within bounds
            if (targetIndex >= Tabs.Count)
            {
                targetIndex = Tabs.Count - 1;
            }

            Tabs.Move(sourceIndex, targetIndex);

            // Update the index property of each tab in the Tabs list and save to database
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Index = i;
            }

            // Assuming you have something on lines of SaveChanges() to reflect the Index change in the DB.
            _assetContext.SaveChanges();
        }
    }

    [RelayCommand]
    private void OnSaveAssets()
    {
        _assetContext.Tabs.Update(SelectedTab);
        _assetContext.SaveChanges();
        MessageBox.Show("Tab saved successfully");
    }

    //// Settings Methods
    //[RelayCommand]
    //private void ToggleSettingsMenu()
    //{

    //    _windowService.ShowDialog<SettingsMenuView, SettingsViewModel>();
    //}
}