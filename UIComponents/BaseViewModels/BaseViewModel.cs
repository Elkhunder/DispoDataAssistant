using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;

namespace DispoDataAssistant.UIComponents.BaseViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly ILogger _logger;
        protected readonly WeakReferenceMessenger _messenger;
        protected readonly IWindowService _windowService;

        [ObservableProperty]
        protected ISnackbarMessageQueue _messageQueue;

        public BaseViewModel(ILogger logger, IWindowService windowService, ISnackbarMessageQueue messageQueue)
        {
            _windowService = windowService;
            _messenger = WeakReferenceMessenger.Default;
            _logger = logger;
            _messageQueue = messageQueue;
        }
    }
}
