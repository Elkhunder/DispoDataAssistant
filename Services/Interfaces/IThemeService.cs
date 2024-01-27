using System.Windows;

namespace DispoDataAssistant.Services.Interfaces
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
