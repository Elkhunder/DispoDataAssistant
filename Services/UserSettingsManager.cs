using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public class UserSettingsManager : IUserSettingsSerivce
    {
        ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>() ?? throw new InvalidOperationException("ISettingsService not registered.");
        IThemesService _themeService = Ioc.Default.GetService<IThemesService>() ?? throw new InvalidOperationException("IThemeService not registered.");
        DataInputViewModel _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>() ?? throw new InvalidOperationException("View Model Locator not registered");


        public void ApplyDeviceManufacturer(string manufacturer)
        {
            _dataInputViewModel.DeviceManufacturer = manufacturer;
        }

        public void ApplyDeviceModel(string deviceModel)
        {
            _dataInputViewModel.DeviceModel = deviceModel;
        }

        public void ApplyDeviceType(string deviceType)
        {
            _dataInputViewModel.DeviceType = deviceType;
        }

        public void ApplyPickupLocation(string pickupLocation)
        {
            _dataInputViewModel.PickupLocation = pickupLocation;
        }

        public void ApplyTheme(string theme)
        {
            _themeService.SetTheme(theme);
        }

        public void ApplyUserSettings(Dictionary<string, object> userSettings)
        {
            object? value;

            if (userSettings.TryGetValue("CurrentTheme", out value))
            {
                ApplyTheme(value?.ToString() ?? "Light");
            }

            if (userSettings.TryGetValue("DeviceType", out value))
            {
                ApplyDeviceType(value?.ToString() ?? "CPU");
            }

            if (userSettings.TryGetValue("DeviceModel", out value))
            {
                ApplyDeviceModel(value?.ToString() ?? "600");
            }

            if (userSettings.TryGetValue("DeviceManufacturer", out value))
            {
                ApplyDeviceManufacturer(value?.ToString() ?? "HP");
            }

            if (userSettings.TryGetValue("PickupLocation", out value))
            {
                ApplyPickupLocation(value?.ToString() ?? "ArborLakes");
            }
        }
    }
}
