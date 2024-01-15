using CommunityToolkit.Mvvm.ComponentModel;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Managers.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DispoDataAssistant.UIComponents.DataInput
{
    public partial class DataInputViewModel : BaseViewModel
    {
        [ObservableProperty]
        TextBox? assetTagTextBox;

        [ObservableProperty]
        private bool? focusTextBox;

        [ObservableProperty]
        string? assetTag;

        [ObservableProperty]
        private string? serialNumber;

        [ObservableProperty]
        private string? deviceType;

        [ObservableProperty]
        private string? deviceManufacturer;

        [ObservableProperty]
        private string? deviceModel;

        [ObservableProperty]
        private string? pickupLocation;

        [ObservableProperty]
        private DateTime? pickupDate;

        [ObservableProperty]
        private List<string>? deviceTypeOptions;

        [ObservableProperty]
        private List<string>? deviceManufacturerOptions;

        [ObservableProperty]
        private List<string>? deviceModelOptions;

        [ObservableProperty]
        private List<string>? pickupLocationOptions;

        [ObservableProperty]
        private ObservableCollection<TabItem>? tabItems;

        private readonly DeviceInformation? _deviceInformation;
        private readonly ITabManager? _tabManager;

        public DataInputViewModel() : this(null!, null!, null!) { }

        public DataInputViewModel(DeviceInformation deviceInformation, ITabManager tabManager, ILogger<DataInputViewModel> logger) : base(logger, null!)
        {
            _deviceInformation = deviceInformation;
            _tabManager = tabManager;

            if (deviceInformation is not null)
            {
                DeviceTypeOptions = _deviceInformation.DeviceTypes;
                DeviceModelOptions = _deviceInformation.DeviceModels;
                DeviceManufacturerOptions = _deviceInformation.DeviceManufacturers;
                PickupLocationOptions = _deviceInformation.PickupLocations;
            }

        }

        public void ClearInputControls()
        {
            AssetTag = string.Empty;
            SerialNumber = string.Empty;
        }

        public void ClearPickupDate()
        {
            PickupDate = null;
        }

        async partial void OnAssetTagChanged(string value)
        {

        }
    }
}
