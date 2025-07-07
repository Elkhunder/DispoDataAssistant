using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.ViewModels;
using System.Windows;
using System.Windows.Input;

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
