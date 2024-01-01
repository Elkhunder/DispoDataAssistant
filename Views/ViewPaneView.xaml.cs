using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Services;
using DispoDataAssistant.ViewModels;
using System.Windows.Controls;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for ViewPaneView.xaml
    /// </summary>
    public partial class ViewPaneView : UserControl
    {
        private readonly ITabManager _tabManager;
        private readonly ViewPaneViewModel _viewModel;
        public ViewPaneView()
        {
            InitializeComponent();

            _tabManager = Ioc.Default.GetRequiredService<ITabManager>();
            _viewModel = Ioc.Default.GetRequiredService<ViewPaneViewModel>();

            this.DataContext = _viewModel;
            ViewPaneTabControl.DataContext = _tabManager;
        }

        private void ViewPaneTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
