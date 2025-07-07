using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System.Windows.Controls;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for DataInputView.xaml
    /// </summary>
    public partial class DataInputView : UserControl
    {
        private readonly DataInputViewModel _dataInputViewModel;
        public DataInputView()
        {
            var serviceProvider = Ioc.Default;
            _dataInputViewModel = serviceProvider.GetRequiredService<DataInputViewModel>();

            InitializeComponent();

            DataContext = _dataInputViewModel;
            _dataInputViewModel.AssetTagTextBox = AssetTagTextBox;
        }
    }
}
