using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System.Windows.Controls;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _mainViewModel;
        public MainView()
        {
            InitializeComponent();

            var serviceProvider = Ioc.Default;

            _mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
            
            DataContext = _mainViewModel;
        }
    }
}
