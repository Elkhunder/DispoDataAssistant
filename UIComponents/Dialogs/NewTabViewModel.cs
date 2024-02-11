using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.UIComponents.Dialogs
{
    public partial class NewTabViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isTabNameToggled = false;
        [ObservableProperty]
        private string title = "Create a New Tab?";
        [ObservableProperty]
        private Visibility toggleVisibility = Visibility.Visible;
        [ObservableProperty]
        private string name;
    }
}
