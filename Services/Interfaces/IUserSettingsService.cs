using System.Collections.Generic;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface IUserSettingsService
    {
        void ApplyUserSettings(Dictionary<string, object> userSettings);
        void ApplyDeviceModel(string deviceModel);
        void ApplyDeviceType(string deviceType);
        void ApplyDeviceManufacturer(string manufacturer);
        void ApplyPickupLocation(string pickupLocation);
        void ApplyTheme(string theme);
    }
}
