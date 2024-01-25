using DispoDataAssistant.Services.Interfaces;
using System.Collections.Generic;

namespace DispoDataAssistant.Services.Implementations;

public class UserSettingsService : IUserSettingsService
{
    //ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>() ?? throw new InvalidOperationException("ISettingsService not registered.");
    //IThemesService _themeService = Ioc.Default.GetService<IThemesService>() ?? throw new InvalidOperationException("IThemeService not registered.");
    //DataInputViewModel _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>() ?? throw new InvalidOperationException("View Model Locator not registered");

    private readonly ISettingsService _settingsService;
    private readonly IThemeService _themeService;

    public UserSettingsService(ISettingsService settingsService, IThemeService themesService)
    {
        _settingsService = settingsService;
        _themeService = themesService;
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
    }
}
