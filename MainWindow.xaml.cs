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

namespace DispoDataAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataInputViewModel _dataInputViewModel;
        public MainWindow()
        {
            InitializeComponent();

            var serviceProvider = Ioc.Default;
            _dataInputViewModel = serviceProvider.GetRequiredService<DataInputViewModel>();
            _dataInputViewModel.AssetTagTextBox.Focus();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Drag the window
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
