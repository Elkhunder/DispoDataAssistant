using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    
    public class DataInputManager : IDataInputService
    {
        ISettingsService _settingsManager = Ioc.Default.GetService<ISettingsService>()!;

        private string? _deviceModel;
        private string? _deviceType;
        private string? _deviceManufacturer;
        private string? _pickupLocation;

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
            get { return (_deviceManufacturer ?? throw new NullReferenceException()); }
            set { _deviceManufacturer = value; }
        }

        public string PickupLocation
        {
            get { return (_pickupLocation ?? throw new NullReferenceException()); }
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
