using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Services
{
    public class WindowManager : BaseService, IWindowService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowManager(IServiceProvider serviceProvider, ILogger<WindowManager> logger) : base(logger)
        {
            _serviceProvider = serviceProvider;
        }

        private Window _mainWindow
        {
            get => Application.Current.MainWindow;
        }
        public void CloseWindow()
        {
            _mainWindow.Close();
        }

        public void MaximizeWindow()
        {
            _mainWindow.WindowState = WindowState.Maximized;
        }

        public void MinimizeWindow()
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }

        public void RestoreWindow()
        {
            _mainWindow.WindowState = WindowState.Normal;
        }

        public TViewModel ShowDialog<TWindow, TViewModel>() where TWindow : Window
        {
            var window = _serviceProvider.GetService<TWindow>();

            if (window is null)
            {
                _logger.LogError("The window instance couldn't be resolved");
                throw new ArgumentNullException(nameof(window), "The window instance couldn't be resolved.");
            }

            window.ShowDialog();
            return (TViewModel)((FrameworkElement)window).DataContext;
            
        }
    }
}
