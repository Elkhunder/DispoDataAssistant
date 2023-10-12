﻿using CommunityToolkit.Mvvm.DependencyInjection;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly SettingsViewModel _settingsViewModel = Ioc.Default.GetService<SettingsViewModel>()!;
        public SettingsView()
        {
            InitializeComponent();
           
            this.DataContext = _settingsViewModel;
            
            _settingsViewModel.ToggleSettingsMenuRequested += OnToggleSettingsMenuRequested;
        }

        private void OnToggleSettingsMenuRequested(object sender, EventArgs e)
        {
            // Perform the animation for the SettingsView UserControl
            // For example, using Storyboard:
            //var storyboard = new Storyboard();
            //var animation = new DoubleAnimation
            //{
            //    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
            //    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            //};
            //if (mainViewModel.IsSettingsMenuVisible)
            //{
            //    animation.From = 0;
            //    animation.To = 1;
            //}
            //else
            //{
            //    animation.From = 1;
            //    animation.To = 0;
            //}
            //Storyboard.SetTarget(animation, this);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));
            //storyboard.Children.Add(animation);
            //storyboard.Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SettingsView loaded.");
            DataContext = _settingsViewModel;
        }

        private void DeviceTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Console.WriteLine($"ComboBox Loaded. Items count: {comboBox.Items.Count}");
            }
        }
    }
}
