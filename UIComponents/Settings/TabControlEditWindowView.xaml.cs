using DispoDataAssistant.Managers.Interfaces;
using System.Windows;

namespace DispoDataAssistant.UIComponents
{
    /// <summary>
    /// Interaction logic for TabControlEditWindowView.xaml
    /// </summary>
    public partial class TabControlEditWindowView : Window
    {
        private readonly TabControlEditViewModel _tabControlEditViewModel;
        public TabControlEditWindowView(TabControlEditViewModel tabControlEditViewModel)
        {
            InitializeComponent();
            _tabControlEditViewModel = tabControlEditViewModel;

            this.DataContext = _tabControlEditViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
