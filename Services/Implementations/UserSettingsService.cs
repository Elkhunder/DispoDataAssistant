using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.DataInput;
using System.Collections.Generic;

namespace DispoDataAssistant.Services.Implementations
{
    public class UserSettingsService : IUserSettingsService
    {
        //ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>() ?? throw new InvalidOperationException("ISettingsService not registered.");
        //IThemesService _themeService = Ioc.Default.GetService<IThemesService>() ?? throw new InvalidOperationException("IThemeService not registered.");
        //DataInputViewModel _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>() ?? throw new InvalidOperationException("View Model Locator not registered");

        private readonly ISettingsService _settingsService;
        private readonly IThemeService _themeService;
        private readonly DataInputViewModel _dataInputViewModel;

        public UserSettingsService(ISettingsService settingsService, IThemeService themesService, DataInputViewModel dataInputViewModel)
        {
            _settingsService = settingsService;
            _themeService = themesService;
            _dataInputViewModel = dataInputViewModel;
        }

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
