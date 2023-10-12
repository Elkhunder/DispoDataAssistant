using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public interface IUserSettingsSerivce
    {
        void ApplyUserSettings(Dictionary<string, object> userSettings);
        void ApplyDeviceModel(string deviceModel);
        void ApplyDeviceType(string deviceType);
        void ApplyDeviceManufacturer(string manufacturer);
        void ApplyPickupLocation(string pickupLocation);
        void ApplyTheme(string theme);
    }
}
