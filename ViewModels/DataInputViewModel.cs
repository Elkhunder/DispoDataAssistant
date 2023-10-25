using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DispoDataAssistant.ViewModels
{
    public partial class DataInputViewModel : BaseViewModel
    {
        private TextBox _assetTagTextBox;
        public TextBox AssetTagTextBox
        {
            get => _assetTagTextBox;
            set => SetProperty(ref _assetTagTextBox, value);
        }
        private bool _focusTextBox;
        public bool FocusTextBox
        {
            get => _focusTextBox;
            set => SetProperty(ref _focusTextBox, value);
        }
        [ObservableProperty]
        string assetTag;
        
        private string? _serialNumber;
        public string SerialNumber
        {
            get => _serialNumber!;
            set => SetProperty(ref _serialNumber, value);
        }
        private string? _deviceType;
        public string DeviceType
        {
            get => _deviceType!;
            set => SetProperty(ref _deviceType, value);
        }

        private string? _deviceManufacturer;
        public string DeviceManufacturer
        {
            get => _deviceManufacturer!;
            set => SetProperty(ref _deviceManufacturer, value);
        }

        private string? _deviceModel;
        public string DeviceModel
        {
            get => _deviceModel!;
            set => SetProperty(ref _deviceModel, value);
        }

        private string? _pickupLocation;
        public string PickupLocation
        {
            get => _pickupLocation!;
            set => SetProperty(ref _pickupLocation, value);
        }
        private DateTime? _pickupDate;
        public DateTime? PickupDate
        {
            get => _pickupDate;
            set => SetProperty(ref _pickupDate, value);
        }

        private List<string> _deviceTypeOptions;
        public List<string> DeviceTypeOptions
        {
            get => _deviceTypeOptions;
            set => SetProperty(ref _deviceTypeOptions, value);
        }

        private List<string> _deviceManufacturerOptions;
        public List<string> DeviceManufacturerOptions
        {
            get => _deviceManufacturerOptions;
            set => SetProperty(ref _deviceManufacturerOptions, value);
        }

        private List<string> _deviceModelOptions;
        public List<string> DeviceModelOptions
        {
            get => _deviceModelOptions;
            set => SetProperty(ref _deviceModelOptions, value);
        }

        private List<string> _pickupLocationOptions;
        public List<string> PickupLocationOptions
        {
            get => _pickupLocationOptions;
            set => SetProperty(ref _pickupLocationOptions, value);
        }

        private DeviceInformation _deviceInformation;
        private readonly IServiceNowApiClient _serviceNowApiClient;

        public DataInputViewModel(IServiceNowApiClient serviceNowApiClient, DeviceInformation deviceInformation)
        {
            _serviceNowApiClient = serviceNowApiClient;
            _deviceInformation = deviceInformation;

            DeviceTypeOptions = _deviceInformation.DeviceTypes!;
            DeviceModelOptions = _deviceInformation.DeviceModels!;
            DeviceManufacturerOptions = _deviceInformation.DeviceManufacturers!;
            PickupLocationOptions = _deviceInformation.PickupLocations!;
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
            //ServiceNowAsset asset = await _serviceNowHandler.GetServiceNowAssetAsync(AssetTag);

            ServiceNowAsset? asset = await _serviceNowApiClient.GetServiceNowAssetAsync(AssetTag);

            if ( asset is not null)
            {
                if (asset.SerialNumber is not null)
                {
                    SerialNumber = asset.SerialNumber;
                }
                if ( asset.Manufacturer is not null)
                {
                    if (asset.Manufacturer is "Hewlett-Packard")
                    {
                        DeviceManufacturer = "HP";
                    }
                    else
                    {
                        DeviceManufacturer = asset.Manufacturer;
                    }
                }
                if (asset.Model is not null)
                {
                    DeviceModel = asset.Model;
                }
                if (asset.Category is not null)
                {
                    DeviceType = asset.Category;
                }
            }
            else
            {
                return;
            }
        }
    }
}
