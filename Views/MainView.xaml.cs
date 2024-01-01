using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _mainViewModel;
        private readonly WindowControlViewModel _windowContolViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly DataActionsViewModel _dataActionsViewModel;
        private readonly DataInputViewModel _dataInputViewModel;
        private readonly Ioc _serviceProvider = Ioc.Default;
        public MainView()
        {
            InitializeComponent();

            _mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            _windowContolViewModel = _serviceProvider.GetRequiredService<WindowControlViewModel>();
            _settingsViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
            _dataActionsViewModel = _serviceProvider.GetRequiredService<DataActionsViewModel>();
            _dataInputViewModel = _serviceProvider.GetRequiredService<DataInputViewModel>();

            
            DataContext = _mainViewModel;
            WindowsControlView.DataContext = _windowContolViewModel;
            DataActionsView.DataContext = _dataActionsViewModel;
            DataInputView.DataContext = _dataInputViewModel;
            SettingsView.DataContext = _settingsViewModel;

            _dataInputViewModel.AssetTagTextBox = DataInputView.AssetTagTextBox;
        }
    }
}
