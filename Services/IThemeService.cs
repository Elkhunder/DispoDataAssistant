using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Services
{
    public interface IThemeService
    {
        string UserTheme { get; }
        void SetTheme(string themeName);
        void UpdateTheme(ResourceDictionary userThemeResource);
        ResourceDictionary GetLoadedTheme();
        void RemoveLoadedTheme(ResourceDictionary loadedThemeResource);
        ResourceDictionary GetThemeResource(string themeName);
    }
}
