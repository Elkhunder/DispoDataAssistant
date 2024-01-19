using System.Windows;
using System.Windows.Controls;

namespace DispoDataAssistant.UIComponents.Settings
{
    /// <summary>
    /// Interaction logic for SettingsMenuView.xaml
    /// </summary>
    public partial class SettingsMenuView : Window
    {
        public SettingsMenuView(MainWindow window, SettingsViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            TabControl tabControl = window.VPTabControl;
        }
    }
}
