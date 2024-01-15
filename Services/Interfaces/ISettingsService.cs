using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface ISettingsService
    {
        string GetTheme();
        void SetTheme(string theme);

        string GetPickupLocation();
        void SetPickupLocation(string pickupLocation);

        string GetDeviceModel();
        void SetDeviceModel(string deviceModel);

        string GetDeviceManufacturer();
        void SetDeviceManufacturer(string deviceManufacturer);

        string GetDeviceType();
        void SetDeviceType(string deviceType);

        Dictionary<string, object> GetAllUserSettings();
    }
}
