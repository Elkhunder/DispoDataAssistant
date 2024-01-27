using DispoDataAssistant.Services.Interfaces;
using System;

namespace DispoDataAssistant.Services.Implementations
{

    public class DataInputService : IDataInputService
    {
        private readonly ISettingsService _settingsManager;

        private string? _deviceModel;
        private string? _deviceType;
        private string? _deviceManufacturer;
        private string? _pickupLocation;

        public DataInputService(ISettingsService settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public string DeviceType
        {
            get { return _deviceType ?? throw new NullReferenceException(); }
            set { _deviceType = value; }
        }

        public string DeviceModel
        {
            get { return _deviceModel ?? throw new NullReferenceException(); }
            set { _deviceModel = value; }
        }

        public string DeviceManufacturer
        {
            get { return _deviceManufacturer ?? throw new NullReferenceException(); }
            set { _deviceManufacturer = value; }
        }

        public string PickupLocation
        {
            get { return _pickupLocation ?? throw new NullReferenceException(); }
            set { _pickupLocation = value; }
        }
        public string GetDeviceManufacturer()
        {
            return _settingsManager.GetDeviceManufacturer();
        }

        public string GetDeviceModel()
        {
            return _settingsManager.GetDeviceModel();
        }

        public string GetDeviceType()
        {
            return _settingsManager.GetDeviceType();
        }

        public string GetPickupLocation()
        {
            return _settingsManager.GetPickupLocation();
        }

        public string SetDeviceManufacturer(string deviceManufacturer)
        {
            throw new NotImplementedException();
        }

        public string SetDeviceModel(string deviceModel)
        {
            throw new NotImplementedException();
        }

        public void SetDeviceType(string deviceType)
        {
            throw new NotImplementedException();
        }

        public string SetPickupLocation(string pickupLocation)
        {
            throw new NotImplementedException();
        }
    }
}
