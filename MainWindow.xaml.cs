using DispoDataAssistant.UIComponents.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DispoDataAssistant;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IServiceProvider _services;
    private MainViewModel _vm;
    public TabControl VPTabControl
    {
        get { return ViewPaneTabControl; }
    }
    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _services = serviceProvider;

        _vm = _services.GetRequiredService<MainViewModel>();
        this.DataContext = _vm;

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

    //private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    //{
    //    _vm.SelectedAsset = null;
    //    if (ViewPaneTabControl.SelectedItem is TabItem tabItem)
    //    {
    //        var dataGrid = FindChild<DataGrid>(tabItem, "ViewPaneDataGrid");

    //        if (dataGrid != null)
    //        {
    //            dataGrid.UnselectAll();
    //        }
    //    }
        
    //}

    private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
    {
        // Confirm parent and childName are valid. 
        if (parent == null) return null;

        T foundChild = null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // If the child is not of the request child type child
            T childType = child as T;
            if (childType == null)
            {
                // Recursively drill down the tree
                foundChild = FindChild<T>(child, childName);

                // If the child is found, break so we do not overwrite the found child. 
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                FrameworkElement frameworkElement = child as FrameworkElement;
                // If the frameworkElement exists and its name is the expected one then it's the requested child
                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    // If the child's name is set for search return the child
                    foundChild = (T)child;
                    break;
                }
            }
            else
            {
                // If the child's name is not set for search return the child.
                foundChild = (T)child;
                break;
            }
        }

        return foundChild;
    }
}
