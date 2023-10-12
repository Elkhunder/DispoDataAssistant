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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DispoDataAssistant.Views
{
    /// <summary>
    /// Interaction logic for DataActionsView.xaml
    /// </summary>
    public partial class DataActionsView : UserControl
    {
        DataActionsViewModel _dataActionsViewModel;
        public DataActionsView()
        {
            _dataActionsViewModel = Ioc.Default.GetService<DataActionsViewModel>()!;
            InitializeComponent();
            DataContext = _dataActionsViewModel;
        }
    }
}
