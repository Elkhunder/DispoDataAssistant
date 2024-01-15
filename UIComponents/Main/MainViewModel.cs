using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using DispoDataAssistant.UIComponents.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Controls;
using System.ComponentModel;

namespace DispoDataAssistant.UIComponents.Main
{
    public partial class MainViewModel : BaseViewModel
    {
        private SettingsViewModel _settingsViewModel;
        private IWindowService _windowService;
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

        public MainViewModel() : this(null!, null!, null!) { }

        public MainViewModel(IWindowService windowService, SettingsViewModel settingsViewModel, ILogger<MainViewModel> logger) : base(logger, null!)
        {
            Console.WriteLine("MainViewModel: Instance Created");
            SettingsViewModel = settingsViewModel;
            _windowService = windowService;


            this.PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void MainViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsSettingsMenuVisible))
            {
                Console.WriteLine($"MainViewModel: IsSettingsMenuVisible changed to: {IsSettingsMenuVisible}");
            }
        }

        [RelayCommand]
        private void CreateNewTab()
        {

        }

        [RelayCommand]
        private void ToggleSettingsMenu()
        {
            Console.WriteLine("MainViewModel: Toggle Settings Menu Requested Event Raised");

            _windowService.ShowDialog<SettingsMenuView, SettingsViewModel>();
        }
    }
}
