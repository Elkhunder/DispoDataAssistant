using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using DispoDataAssistant.UIComponents.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.UIComponents.Main;

public partial class MainViewModel : BaseViewModel
{
    // Binding properties
    [ObservableProperty]
    private ObservableCollection<TabModel> tabs = [];
    [ObservableProperty]
    private TabModel selectedTab = new();
    [ObservableProperty]
    private int selectedTabIndex;
    [ObservableProperty]
    private ServiceNowAsset selectedAsset = new();
    [ObservableProperty]
    private string serialNumber = string.Empty;
    [ObservableProperty]
    private string assetTag = string.Empty;

    // DI Injections
    private AssetContext _assetContext;
    private TabControlEditWindowView _tabControlEditWindow;
    private IServiceNowApiClient _serviceNowApiClient;
    private IFileDialogService _fileDialogService;

    public MainViewModel() : this(null!, null!, null!, null!, null!) { }

    public MainViewModel(ILogger<MainViewModel> logger, AssetContext assetContext, TabControlEditWindowView tabControlEditWindow, IServiceNowApiClient serviceNowApiClient, IFileDialogService fileDialogService) : base(logger, null!)
    {
        Console.WriteLine("MainViewModel: Instance Created");
        _assetContext = assetContext;
        _tabControlEditWindow = tabControlEditWindow;
        _serviceNowApiClient = serviceNowApiClient;
        _fileDialogService = fileDialogService;
    }
    
    // Event Methods
    [RelayCommand]
    private void MainWindow_Loaded()
    {
        if (Task.Run(async () => await _assetContext.Tabs.AnyAsync()).Result)
        {
            _assetContext.Tabs.Load();
            Tabs = _assetContext.Tabs.Local.ToObservableCollection();
        }
    }

    // Service Now Query Methods
    [RelayCommand]
    private async Task QueryServiceNow()
    {
        // Prioritize the use of serial number as it's more accurate
        if (!string.IsNullOrEmpty(SerialNumber))
        {
            // Query service now asset table with serial number
            var asset = await _serviceNowApiClient.GetServiceNowAssetBySerialNumberAsync(SerialNumber);

            if (asset is null)
            {
                MessageBox.Show("Asset was not found, please enter information manually");
                return;
            }
            else if (SelectedTab.ServiceNowAssets is null)
            {
                MessageBox.Show("Asset collection is null, unable to add asset to collection");
            }
            else if (asset.SerialNumber == SerialNumber)
            {
                SelectedTab.ServiceNowAssets?.Add(asset);
                _assetContext.SaveChanges();
            }
            else
            {
                MessageBox.Show("The provided serial number did not match the serial number returned by service now");
            }
        }
        else if (!string.IsNullOrEmpty(AssetTag))
        {
            // Query service now asset table with asset tag as a fallback if serial number is empty
            var asset = await _serviceNowApiClient.GetServiceNowAssetByAssetTagAsync(AssetTag);

            if (asset is null)
            {
                MessageBox.Show("Asset was not found, please enter information manually");
                return;
            }
            else if (SelectedTab.ServiceNowAssets is null)
            {
                MessageBox.Show("Asset collection is null, unable to add asset to collection");
            }
            else if (asset.AssetTag == AssetTag)
            {
                SelectedTab.ServiceNowAssets?.Add(asset);
                _assetContext.SaveChanges();
            }
            else
            {
                MessageBox.Show("The provided asset tag didn't match the asset tag returned by service now");
            }
        }
        else
        {
            // Do something if both serial number and asset tag are empty or null
        }
    }

    // Tab Action Methods
    [RelayCommand]
    private void CreateTab()
    {
        var newTabNameRequest = new RequestNewTabNameMessage();
        _messenger.Send(newTabNameRequest);
        if (newTabNameRequest.HasReceivedResponse)
        {
            string name = newTabNameRequest.Response;

            var tab = new TabModel() { Name = name, ServiceNowAssets = [] };
            _assetContext.Tabs.Add(tab);
            _assetContext.SaveChanges();
        }
    }

    [RelayCommand]
    private void DeleteTab()
    {
        _assetContext.Tabs.Remove(SelectedTab);
        _assetContext.SaveChanges();
    }


    [RelayCommand]
    private void RenameTab()
    {
        //tab name is not updating in real time
        var tab = SelectedTab;

        var newTabNameRequest = _messenger.Send(new RequestNewTabNameMessage());

        
        if (newTabNameRequest.HasReceivedResponse)
        {
            var newTabName = newTabNameRequest.Response;
            if (string.IsNullOrEmpty(newTabName))
            {
                MessageBox.Show("New Tab Name was empty when attempting to change tab name");
                return;
            }
            else if (SelectedTab.Name == newTabName)
            {
                MessageBox.Show("New tab name was the same as the current tab name");
                return;
            }
            _assetContext.Tabs.Update(SelectedTab);
        }
        _assetContext.SaveChanges();
    }

    [RelayCommand]
    private void UploadAssets()
    {
        var file = _fileDialogService.OpenFileDialog();
    }

    [RelayCommand]
    private void DownloadAssets()
    {
        var file = _fileDialogService.SaveFileDialog();
        var tab = SelectedTab;

        var assets = tab.ServiceNowAssets;

        using (StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write)))
        {
            CsvHandler csvHandler = new CsvHandler();
            string delimeter = "\t";
            writer.Write(csvHandler.BuildCsvHeader(delimeter));

            foreach (var asset in assets)
            {
                string line = csvHandler.ConvertAssetToCsvLine(asset, delimeter);
                writer.WriteLine(line);
            }
        }
    }
        

    [RelayCommand]
    private void SaveAssets()
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