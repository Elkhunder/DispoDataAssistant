using DispoDataAssistant.UIComponents.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DispoDataAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _services;
        public TabControl VPTabControl
        {
            get { return ViewPaneTabControl; }
        }
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _services = serviceProvider;

            MainViewModel vm = _services.GetRequiredService<MainViewModel>();
            this.DataContext = vm;

            //var mainView = _services.GetRequiredService<MainView>();
            //mainView.Height = this.Height;
            //mainView.Width = this.Width;

            //this.Content = mainView;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Drag the window
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
