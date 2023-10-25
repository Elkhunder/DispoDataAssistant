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
