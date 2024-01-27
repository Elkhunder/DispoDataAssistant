using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DispoDataAssistant.UIComponents.BaseViewModels
{
    public class BaseViewModel : ObservableObject
    {
        protected readonly ILogger _logger;
        protected readonly WeakReferenceMessenger _messenger;
        protected readonly IWindowService _windowService;

        public BaseViewModel(ILogger logger, IWindowService windowService)
        {
            _windowService = windowService;
            _messenger = WeakReferenceMessenger.Default;
            _logger = logger;
        }
    }
}
