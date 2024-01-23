using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Messages;
using DispoDataAssistant.UIComponents.BaseViewModels;
using DispoDataAssistant.UIComponents.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.UIComponents.Main;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private SettingsViewModel settingsViewModel;
    [ObservableProperty]
    private ObservableCollection<TabModel> tabs = [];
    [ObservableProperty]
    private TabModel selectedTab = new();
    [ObservableProperty]
    private int selectedTabIndex;
    [ObservableProperty]
    private ServiceNowAsset selectedAsset = new();

    private AssetContext _assetContext;
    private TabControlEditWindowView _tabControlEditWindow;

    public MainViewModel() : this(null!, null!, null!, null!) { }

    public MainViewModel(SettingsViewModel settingsViewModel, ILogger<MainViewModel> logger, AssetContext assetContext, TabControlEditWindowView tabControlEditWindow) : base(logger, null!)
    {
        Console.WriteLine("MainViewModel: Instance Created");
        SettingsViewModel = settingsViewModel;
        _assetContext = assetContext;
        _tabControlEditWindow = tabControlEditWindow;
    }
    

    [RelayCommand]
    private void MainWindow_Loaded()
    {
        if (Task.Run(async () => await _assetContext.Tabs.AnyAsync()).Result)
        {
            _assetContext.Tabs.Load();
            Tabs = _assetContext.Tabs.Local.ToObservableCollection();
        }
    }

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

    }

    [RelayCommand]
    private void DownloadAssets()
    {
        var tab = SelectedTab;

        var assets = tab.ServiceNowAssets?.ToList();
    }

    [RelayCommand]
    private void SaveAssets()
    {

        //_tabContext.SaveChanges();
        if (SelectedAsset is null)
        {
            MessageBox.Show("Unable to save changes, selected asset was null");
            return;
        }

        _assetContext.Tabs.Update(SelectedTab);
        _assetContext.SaveChanges();
        MessageBox.Show("Tab saved successfully");
    }

    [RelayCommand]
    private void ToggleSettingsMenu()
    {

        _windowService.ShowDialog<SettingsMenuView, SettingsViewModel>();
    }
}