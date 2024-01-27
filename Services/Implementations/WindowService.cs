using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace DispoDataAssistant.Services.Implementations
{
    public class WindowService : BaseService, IWindowService
    {
        private readonly IServiceProvider _serviceProvider;
        public WindowService(IServiceProvider serviceProvider, ILogger<WindowService> logger) : base(logger)
        {
            _serviceProvider = serviceProvider;
        }

        private static Window MainWindow
        {
            get => Application.Current.MainWindow;
        }
        public void CloseWindow()
        {
            MainWindow.Close();
        }

        public void MaximizeWindow()
        {
            MainWindow.WindowState = WindowState.Maximized;
        }

        public void MinimizeWindow()
        {
            MainWindow.WindowState = WindowState.Minimized;
        }

        public void RestoreWindow()
        {
            MainWindow.WindowState = WindowState.Normal;
        }

        public TViewModel ShowDialog<TWindow, TViewModel>() where TWindow : Window
        {
            TWindow? window = _serviceProvider.GetService<TWindow>();

            if (window is null)
            {
                _logger.LogError("The window instance couldn't be resolved");
                throw new ArgumentNullException(nameof(window), "The window instance couldn't be resolved.");
            }

            window.ShowDialog();
            return (TViewModel)window.DataContext;

        }
    }
}
