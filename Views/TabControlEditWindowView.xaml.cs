using DispoDataAssistant.Services;
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
using System.Windows.Shapes;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for TabControlEditWindowView.xaml
    /// </summary>
    public partial class TabControlEditWindowView : Window
    {
        private ITabManager _tabManager;
        private TabControlEditViewModel _tabControlEditViewModel;
        public TabControlEditWindowView(ITabManager tabManager, TabControlEditViewModel tabControlEditViewModel)
        {
            InitializeComponent();
            _tabManager = tabManager;
            _tabControlEditViewModel = tabControlEditViewModel;

            this.DataContext = _tabControlEditViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
