using System.Collections.Generic;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface IUserSettingsService
    {
        void ApplyUserSettings(Dictionary<string, object> userSettings);
        void ApplyTheme(string theme);
    }
}
