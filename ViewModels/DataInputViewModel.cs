using CommunityToolkit.Mvvm.ComponentModel;
using DispoDataAssistant.Enums;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DispoDataAssistant.ViewModels
{
    public partial class DataInputViewModel : BaseViewModel
    {
        private TextBox? _assetTagTextBox;
        public TextBox? AssetTagTextBox
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
        public string? SerialNumber
        {
            get => _serialNumber;
            set => SetProperty(ref _serialNumber, value);
        }
        private string? _deviceType;
        public string? DeviceType
        {
            get => _deviceType;
            set => SetProperty(ref _deviceType, value);
        }

        private string? _deviceManufacturer;
        public string? DeviceManufacturer
        {
            get => _deviceManufacturer;
            set => SetProperty(ref _deviceManufacturer, value);
        }

        private string? _deviceModel;
        public string? DeviceModel
        {
            get => _deviceModel;
            set => SetProperty(ref _deviceModel, value);
        }

        private string? _pickupLocation;
        public string? PickupLocation
        {
            get => _pickupLocation;
            set => SetProperty(ref _pickupLocation, value);
        }
        private DateTime? _pickupDate;
        public DateTime? PickupDate
        {
            get => _pickupDate;
            set => SetProperty(ref _pickupDate, value);
        }

        private List<string>? _deviceTypeOptions;
        public List<string>? DeviceTypeOptions
        {
            get => _deviceTypeOptions;
            set => SetProperty(ref _deviceTypeOptions, value);
        }

        private List<string>? _deviceManufacturerOptions;
        public List<string>? DeviceManufacturerOptions
        {
            get => _deviceManufacturerOptions;
            set => SetProperty(ref _deviceManufacturerOptions, value);
        }

        private List<string>? _deviceModelOptions;
        public List<string>? DeviceModelOptions
        {
            get => _deviceModelOptions;
            set => SetProperty(ref _deviceModelOptions, value);
        }

        private List<string>? _pickupLocationOptions;
        public List<string>? PickupLocationOptions
        {
            get => _pickupLocationOptions;
            set => SetProperty(ref _pickupLocationOptions, value);
        }

        private DeviceInformation _deviceInformation;
        private readonly IServiceNowApiClient _serviceNowApiClient;
        private readonly ILogger<DataInputViewModel> _logger;

        public DataInputViewModel(IServiceNowApiClient serviceNowApiClient, DeviceInformation deviceInformation, ILogger<DataInputViewModel> logger)
        {
            _serviceNowApiClient = serviceNowApiClient;
            _deviceInformation = deviceInformation;
            _logger = logger;

            DeviceTypeOptions = _deviceInformation.DeviceTypes;
            DeviceModelOptions = _deviceInformation.DeviceModels;
            DeviceManufacturerOptions = _deviceInformation.DeviceManufacturers;
            PickupLocationOptions = _deviceInformation.PickupLocations;

            assetTag = string.Empty;
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
            ServiceNowAsset? existingAsset = DbService.GetServiceNowAssetByProperty("AssetTag", value);

            if (existingAsset is not null)
            {
                Enum.TryParse<ServiceNowInstallStatus>(existingAsset.InstallStatus, out ServiceNowInstallStatus installStatus);
                if (installStatus is ServiceNowInstallStatus.Retired)
                {

                    AssignAssetValuesToFields(existingAsset);
                    return;
                }
            }

            ServiceNowAsset? asset = await _serviceNowApiClient.GetServiceNowAssetAsync(AssetTag);

            

            if ( asset is not null)
            {
                ServiceNowAsset? serviceNowAsset = DbService.SaveAsset(asset, true);
                if ( serviceNowAsset is not null )
                {
                    SerialNumber = serviceNowAsset.SerialNumber;

                    if (asset.Manufacturer is "Hewlett-Packard")
                    {
                        DeviceManufacturer = "HP";
                    }
                    else
                    {
                        DeviceManufacturer = asset.Manufacturer;
                    }

                    DeviceModel = asset.Model;
                    DeviceType = asset.Category;
                }
            }
            else
            {
                _logger.LogInformation($"Asset {value} was not found in service now");
                return;
            }
        }

        private void AssignAssetValuesToFields(ServiceNowAsset asset)
        {
            SerialNumber = asset.SerialNumber;
            DeviceManufacturer = asset.Manufacturer;
            DeviceModel = asset.Model;
            DeviceType = asset.Category;

        }
    }
}
