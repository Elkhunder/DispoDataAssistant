using System.Windows;

namespace DispoDataAssistant.Services.Interfaces
{
    public interface IWindowService
    {
        void CloseWindow();
        void MaximizeWindow();
        void MinimizeWindow();
        void RestoreWindow();
        TViewModel ShowDialog<TWindow, TViewModel>() where TWindow : Window;
    }
}
