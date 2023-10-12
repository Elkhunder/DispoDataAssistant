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
    /// Interaction logic for DataInputView.xaml
    /// </summary>
    public partial class DataInputView : UserControl
    {
        private readonly DataInputViewModel _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>()!;
        public DataInputView()
        {
            InitializeComponent();
            this.DataContext = _dataInputViewModel;
            _dataInputViewModel.AssetTagTextBox = AssetTagTextBox;
        }
    }
}
