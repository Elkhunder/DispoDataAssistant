using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private SettingsViewModel _settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get => _settingsViewModel;
            set => SetProperty(ref _settingsViewModel, value);
        }
        private bool _isSettingsMenuVisible = false;
        public bool IsSettingsMenuVisible
        {
            get => _isSettingsMenuVisible;
            set => SetProperty(ref _isSettingsMenuVisible, value);
        }

        public MainViewModel(SettingsViewModel settingsViewModel, ILogger<MainViewModel> logger) : base(logger)
        {
            var provider = Ioc.Default;
            Console.WriteLine("MainViewModel: Instance Created");
            SettingsViewModel = settingsViewModel;
            SettingsViewModel.ToggleSettingsMenuRequested += OnToggleSettingsMenuRequested;

            this.PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void MainViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsSettingsMenuVisible))
            {
                Console.WriteLine($"MainViewModel: IsSettingsMenuVisible changed to: {IsSettingsMenuVisible}");
            }
        }

        private void OnToggleSettingsMenuRequested(object sender, EventArgs e)
        {
            Console.WriteLine("MainViewModel: Toggle Settings Menu Requested Event Raised");
            IsSettingsMenuVisible = !IsSettingsMenuVisible;
        }
    }
}
