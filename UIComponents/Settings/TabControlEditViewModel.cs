using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Windows;

namespace DispoDataAssistant.UIComponents;

public partial class TabControlEditViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool isInvalid;
    [ObservableProperty]
    private string? newTabName;

    [ObservableProperty]
    private string? currentTabName;

    [ObservableProperty]
    private string? deviceId;

    [ObservableProperty]
    private string? fileName;

    [ObservableProperty]
    private string? pickupLocation;

    [ObservableProperty]
    private string? pickupDate;

    [ObservableProperty]
    private List<string>? pickupLocationOptions;

    [ObservableProperty]
    private Visibility newTabNamePanelVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility deviceIdPanelVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility fileNamePanelVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility currentTabNamePanelVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility pickupLocationDateVisibility = Visibility.Collapsed;


    public TabControlEditViewModel() : this(null!, null!, null!) { }

    public TabControlEditViewModel(ILogger<TabControlEditViewModel> logger, IWindowService windowService, DeviceInformation deviceInformation) : base(logger, windowService)
    {
        PickupLocationOptions = deviceInformation.PickupLocations;

        RegisterMessengers();
    }

    private void RegisterMessengers()
    {
        _messenger.Register<TabControlEditViewModel, RequestNewTabNameMessage>(this, OnRequestNewTabNameReceived);
    }

    private void OnRequestNewTabNameReceived(TabControlEditViewModel recipient, RequestNewTabNameMessage message)
    {
        NewTabNamePanelVisibility = Visibility.Visible;
        TabControlEditViewModel vm = _windowService.ShowDialog<TabControlEditWindowView, TabControlEditViewModel>();

        if (vm.NewTabName is not null)
        {
            message.Reply(vm.NewTabName);
        }
        NewTabName = null;
    }

    [RelayCommand]
    private void AcceptButton_OnClick(Window? window)
    {
        if (IsInvalid)
        {
            MessageBox.Show("Tab Name is Invalid!");

        }
        else
        {
            window?.Close();
        }
    }
}
