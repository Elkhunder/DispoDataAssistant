using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace DispoDataAssistant.ViewModels
{
    //TODO: Close Window Command (logic and binding)
    //      Maximize Window Command (logic and binding)
    //      Restore Window Command (logic and binding)
    //      Minimize Window Command (logic and binding)

    [ObservableRecipient]
    public partial class WindowControlViewModel : BaseViewModel
    {
        private readonly IWindowService _windowManager;
        public RelayCommand ToggleSettingsMenuCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand RestoreWindowCommand { get; set; }

        private Visibility _maximizeWindowVisible = Visibility.Visible;
        public Visibility MaximizeWindowVisible
        {
            get => _maximizeWindowVisible;
            set => SetProperty(ref _maximizeWindowVisible, value);
        }
        private Visibility _restoreWindowVisible = Visibility.Collapsed;
        public Visibility RestoreWindowVisible
        {
            get => _restoreWindowVisible;
            set => SetProperty(ref _restoreWindowVisible, value);
        }
        public WindowControlViewModel() : this(null!, null!) { }
        public WindowControlViewModel(IWindowService windowService, ILogger<WindowControlViewModel> logger) : base(logger)
        {
            _logger?.LogInformation("Instance Created");
            _windowManager = windowService;
            ToggleSettingsMenuCommand = new RelayCommand(ToggleSettingsMenu);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            MaximizeWindowCommand = new RelayCommand(MaximizeWindow);
            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            RestoreWindowCommand = new RelayCommand(RestoreWindow);
        }

        private void MinimizeWindow()
        {
            _windowManager.MinimizeWindow();
        }

        private void MaximizeWindow()
        {
            _windowManager.MaximizeWindow();
            RestoreWindowVisible = Visibility.Visible;
            MaximizeWindowVisible = Visibility.Collapsed;
        }

        private void CloseWindow()
        {
            _windowManager.CloseWindow();
        }
        private void RestoreWindow()
        {
            _windowManager.RestoreWindow();
            MaximizeWindowVisible = Visibility.Visible;
            RestoreWindowVisible = Visibility.Collapsed;
        }

        private void ToggleSettingsMenu()
        {
            _logger?.LogInformation("Toggle Settings Menu Command Executed");
            WeakReferenceMessenger.Default.Send(new ToggleSettingsMenuMessage());
        }
    }
}
