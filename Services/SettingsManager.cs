using System;
using System.Collections.Generic;
using DispoDataAssistant.Properties;

namespace DispoDataAssistant.Services
{
    public class SettingsManager : ISettingsService
    {
        public Dictionary<string, object> GetAllUserSettings()
        {
            var userSettings = new Dictionary<string, object>
            {
                { "CurrentTheme", GetTheme() },
                { "DeviceType", GetDeviceType() },
                { "DeviceModel", GetDeviceModel() },
                { "DeviceManufacturer", GetDeviceManufacturer() },
                { "PickupLocation", GetPickupLocation() },
            };

            return userSettings;
        }

        public string GetDeviceManufacturer()
        {
            return UserSettings.Default.DeviceManufacturer;
        }

        public string GetDeviceModel()
        {
            return UserSettings.Default.DeviceModel;
        }

        public string GetDeviceType()
        {
            return UserSettings.Default.DeviceType;
        }

        public string GetPickupLocation()
        {
            return UserSettings.Default.PickupLocation;
        }

        public string GetTheme()
        {
            return UserSettings.Default.Theme;
        }

        public void SetDeviceManufacturer(string deviceManufacturer)
        {
            UserSettings.Default.DeviceManufacturer = deviceManufacturer;
            UserSettings.Default.Save();
        }

        public void SetDeviceModel(string deviceModel)
        {
            UserSettings.Default.DeviceModel = deviceModel;
            UserSettings.Default.Save();
        }

        public void SetDeviceType(string deviceType)
        {
            UserSettings.Default.DeviceType = deviceType;
            UserSettings.Default.Save();
        }

        public void SetPickupLocation(string pickupLocation)
        {
            UserSettings.Default.PickupLocation = pickupLocation;
            UserSettings.Default.Save();
        }

        public void SetTheme(string theme)
        {
            UserSettings.Default.Theme = theme;
            UserSettings.Default.Save();
        }
    }
}
