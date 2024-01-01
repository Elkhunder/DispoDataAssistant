using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.ViewModels
{
    public partial class TabControlEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isInvalid;
        [ObservableProperty]
        private string? newTabName;

        [ObservableProperty]
        private string? currentTabName;

        [ObservableProperty]
        private string? deviceId;

        [ObservableProperty]
        private string? fileName;

        [ObservableProperty]
        private Visibility newTabNamePanelVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility deviceIdPanelVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility fileNamePanelVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility currentTabNamePanelVisibility = Visibility.Collapsed;


        public TabControlEditViewModel() : this(null!) { }

        public TabControlEditViewModel(ILogger<TabControlEditViewModel> logger) : base(logger)
        {
        }

        [RelayCommand]
        private void AcceptButton_OnClick(Window? window)
        {
            if (IsInvalid)
            {
                MessageBox.Show("Tab Name is Invalid!");
                
            }
            else
            {
                if (window != null)
                {
                    window.Close();
                }
            }
        }
    }
}
