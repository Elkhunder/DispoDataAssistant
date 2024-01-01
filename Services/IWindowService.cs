using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Services
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
