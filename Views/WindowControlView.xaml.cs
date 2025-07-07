using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System.Windows.Controls;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for WindowControlView.xaml
    /// </summary>
    public partial class WindowControlView : UserControl
    {
        private readonly WindowControlViewModel _windowContolViewModel;
        public WindowControlView()
        {
            InitializeComponent();

            var serviceProvider = Ioc.Default;
            _windowContolViewModel = serviceProvider.GetRequiredService<WindowControlViewModel>();
            DataContext = _windowContolViewModel;
        }
    }
}
