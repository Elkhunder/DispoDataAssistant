﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CsvHelper;
using CsvHelper.Configuration;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
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
            var tabs = _assetContext.Tabs.Local.ToList();

            bool isIndexUpdated = false;

            if (tabs != null && tabs.Count > 0)
            {
                for (int i = 0; i < tabs.Count; i++)
                {
                    var tab = tabs[i];

                    // If Index is null
                    if (tab.Index == null)
                    {
                        tabs[i].Index = i;
                        isIndexUpdated = true;
                    }
                }
                if (isIndexUpdated)
                {
                    _assetContext.SaveChanges();
                }

                Tabs = new ObservableCollection<TabModel>(tabs.OrderBy(tab => tab.Index));
            }
        }
    }

    // Service Now Query Methods
    [RelayCommand]
    private async Task QueryServiceNow()
    {
        if (SelectedTab.ServiceNowAssets is null)
        {
            SelectedTab.ServiceNowAssets = [];
        }

        // Prioritize the use of serial number as it's more accurate
        if (!string.IsNullOrEmpty(SerialNumber))
        {
            // Query service now asset table with serial number
            var apiResponse = await _serviceNowApiClient.GetServiceNowAssetBySerialNumberAsync(SerialNumber);

            if(apiResponse.IsSuccessful is false || apiResponse.Data is null)
            {
                MessageBox.Show("Query was not successful");
                _logger.LogError($"Query was not successful, {apiResponse.ErrorMessage}");
                return;
            }
            
            if (apiResponse.Data.Assets.Count == 0)
            {
                _logger.LogError($"No assets were found with serial number: {SerialNumber}");
                return;
            }
            var assets = apiResponse.Data.Assets;
            var hasDuplicates = assets.Count != assets.Distinct().Count();
            if (hasDuplicates)
            {
                assets = assets.GroupBy(asset => asset)
               .Select(group => group.First())
               .ToList();
            }

            foreach ( var asset in assets)
            {
                if (asset.SerialNumber == SerialNumber)
                {
                    SelectedTab.ServiceNowAssets?.Add(asset);
                    _assetContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("The provided serial number did not match the serial number returned by service now");
                }
            }
        }
        else if (!string.IsNullOrEmpty(AssetTag))
        {
            // Query service now asset table with asset tag as a fallback if serial number is empty
            var apiResponse = await _serviceNowApiClient.GetServiceNowAssetByAssetTagAsync(AssetTag);
            if (apiResponse.IsSuccessful is false || apiResponse.Data is null)
            {
                MessageBox.Show("Query was not successful");
                _logger.LogError($"Query was not successful, {apiResponse.ErrorMessage}");
                return;
            }
            if (apiResponse.Data.Assets.Count == 0)
            {
                MessageBox.Show("No assets found, please enter information manually");
                return;
            }
            var assets = apiResponse.Data.Assets;
            var hasDuplicates = assets.Count != assets.Distinct().Count();
            if (hasDuplicates)
            {
                assets = assets.GroupBy(asset => asset)
               .Select(group => group.First())
               .ToList();
            }

            foreach ( var asset in assets)
            {
                if (asset is null)
                {
                    _logger.LogError($"{asset} was not found in Service Now");
                    return;
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
            SelectedTab.Name = newTabName;
            _assetContext.Tabs.Update(SelectedTab);
        }
        _assetContext.SaveChanges();
    }

    [RelayCommand]
    private async Task UploadAssets()
    {
        var newTabNameRequest = new RequestNewTabNameMessage();
        _messenger.Send(newTabNameRequest);
        var tab = new TabModel();

        if (newTabNameRequest.HasReceivedResponse)
        {
            string tabName = newTabNameRequest.Response;

            if (tabName is null)
            {
                tab = SelectedTab;
            }
            else
            {
                tab = new TabModel() { Name = tabName, ServiceNowAssets = [] };
                _assetContext.Tabs.Add(tab);
            }
        }
        else
        {
            tab = SelectedTab;
        }
        OpenFileDialog openFileDialog = new OpenFileDialog();

        bool? result = openFileDialog.ShowDialog();

        if (result == false)
        {
            _logger.LogInformation("Operation canceled by user");
            return;
        }
        var file = openFileDialog.FileName;
        if (string.IsNullOrEmpty(file))
        {
            _logger.LogError("file was null or empty");
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

            foreach (var record in records)
            {
                if (record.SysId is null || string.IsNullOrEmpty(record.SysId))
                {
                    RestResponse<ServiceNowApiResponse>? apiResponse = null;
                    bool requestPerformed = false;

                    if (record.SerialNumber is not null && !string.IsNullOrEmpty(record.SerialNumber))
                    {
                        apiResponse = await _serviceNowApiClient.GetServiceNowAssetBySerialNumberAsync(record.SerialNumber);
                        requestPerformed = true;
                    }
                    else if (record.AssetTag is not null && !string.IsNullOrEmpty(record.AssetTag))
                    {
                        apiResponse = await _serviceNowApiClient.GetServiceNowAssetByAssetTagAsync(record.AssetTag);
                        requestPerformed = true;
                    }

                    if ( requestPerformed && ( apiResponse is null || apiResponse.IsSuccessful is false || apiResponse.Data is null ))
                    {
                        MessageBox.Show("Query was not successful");
                        string errorMessage = apiResponse?.ErrorMessage ?? "API Response or Data was null";
                        _logger.LogError($"Query was not successful, {errorMessage}");
                        return;
                    }
                    else if (apiResponse!.Data!.Assets.Count == 0)
                    {
                        MessageBox.Show("No assets found, please enter information manually");
                        return;
                    }
                    var assets = apiResponse.Data.Assets;
                    var hasDuplicates = assets.Count != assets.Distinct().Count();
                    if (hasDuplicates)
                    {
                        assets = assets.GroupBy(asset => asset)
                       .Select(group => group.First())
                       .ToList();
                    }

                    foreach (var asset in assets)
                    {
                        if (asset is null)
                        {
                            _logger.LogError($"{asset} was not found in Service Now");
                            return;
                        }
                        else if (asset.AssetTag == record.AssetTag || asset.SerialNumber == record.SerialNumber)
                        {
                            tab.ServiceNowAssets?.Add(asset);
                        }
                        else
                        {
                            MessageBox.Show("The provided asset tag didn't match the asset tag returned by service now");
                        }
                    }
                }
                else
                {
                    record.Tab = tab;
                    record.TabId = tab.Id;
                    tab.ServiceNowAssets?.Add(record);
                }
            }
        }
        _assetContext.SaveChanges();
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

    public void DragOver(IDropInfo dropInfo)
    {
        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        dropInfo.Effects = DragDropEffects.Move;
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is TabModel sourceItem)
        {
            var sourceIndex = Tabs.IndexOf(sourceItem);
            var targetIndex = dropInfo.InsertIndex < Tabs.Count ? dropInfo.InsertIndex : Tabs.Count - 1;

            Tabs.Move(sourceIndex, targetIndex);
            SelectedTabIndex = targetIndex; // optional - to select the tab after moving
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