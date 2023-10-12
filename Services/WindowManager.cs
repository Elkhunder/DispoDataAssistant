using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Services
{
    public class WindowManager : IWindowService
    {
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
    }
}
