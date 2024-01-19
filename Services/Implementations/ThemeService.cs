using DispoDataAssistant.Services.Interfaces;
using System;
using System.Linq;
using System.Windows;

namespace DispoDataAssistant.Services.Implementations
{
    public class ThemeService : IThemeService
    {
        private readonly ResourceDictionary? _loadedThemeResource;
        //ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>();
        private readonly ISettingsService _settingsService;
        public ThemeService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public string UserTheme => _settingsService.GetTheme();

        public ResourceDictionary GetLoadedTheme()
        {
            System.Collections.ObjectModel.Collection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            return mergedDictionaries.FirstOrDefault(dictionary => dictionary.Source.OriginalString.EndsWith("Theme.xaml"));
        }

        public ResourceDictionary GetThemeResource(string themeName)
        {
            string themePath = $"Resources/Themes/{themeName}Theme.xaml";

            ResourceDictionary themeResource = new()
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            return themeResource;
        }


        public void SetTheme(string themeName)
        {
            if (string.IsNullOrWhiteSpace(themeName))
            {
                themeName = "Light";
            }


            ResourceDictionary userThemeResource = GetThemeResource(themeName);

            RemoveLoadedTheme();

            UpdateTheme(userThemeResource);
        }

        public static void RemoveLoadedTheme()
        {
            App.Current.Resources.Clear();
        }
        public void UpdateTheme(ResourceDictionary userThemeResource)
        {
            System.Collections.ObjectModel.Collection<ResourceDictionary> appMergedDictionaries = App.Current.Resources.MergedDictionaries;

            appMergedDictionaries.Add(userThemeResource);
        }

        public void RemoveLoadedTheme(ResourceDictionary loadedThemeResource)
        {
            throw new NotImplementedException();
        }
    }
}
