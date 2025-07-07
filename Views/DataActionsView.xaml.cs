using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System.Windows.Controls;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for DataActionsView.xaml
    /// </summary>
    public partial class DataActionsView : UserControl
    {
        private readonly DataActionsViewModel _dataActionsViewModel;
        public DataActionsView()
        {
            var serviceProvider = Ioc.Default;
            _dataActionsViewModel = serviceProvider.GetRequiredService<DataActionsViewModel>();
            InitializeComponent();

            DataContext = _dataActionsViewModel;
        }
    }
}
