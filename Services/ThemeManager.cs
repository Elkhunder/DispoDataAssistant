using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Services
{
    public class ThemeManager : IThemeService
    {
        private ResourceDictionary? _loadedThemeResource;
        //ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>();
        private readonly ISettingsService _settingsService;
        public ThemeManager(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public string UserTheme => _settingsService.GetTheme();

        public ResourceDictionary GetLoadedTheme()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            return mergedDictionaries.FirstOrDefault(dictionary => dictionary.Source.OriginalString.EndsWith("Theme.xaml"));
        }

        public ResourceDictionary GetThemeResource(string themeName)
        {
            var themePath = $"Resources/Themes/{themeName}Theme.xaml";

            ResourceDictionary themeResource = new ResourceDictionary();
            themeResource.Source = new Uri(themePath, UriKind.Relative);

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

        public void RemoveLoadedTheme()
        {
            App.Current.Resources.Clear();
        }
        public void UpdateTheme(ResourceDictionary userThemeResource)
        {
            var appMergedDictionaries = App.Current.Resources.MergedDictionaries;

            appMergedDictionaries.Add(userThemeResource);
        }

        public void RemoveLoadedTheme(ResourceDictionary loadedThemeResource)
        {
            throw new NotImplementedException();
        }
    }
}
