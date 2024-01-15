using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents;
using Microsoft.Extensions.Logging;
using System;

namespace DispoDataAssistant.Services.Implementations
{
    public class DataExportService
    {
        private readonly string _fileType = string.Empty;
        private readonly IWindowService _windowService;
        private readonly TabControlEditViewModel _tabControlEditViewModel;
        private readonly ILogger _logger;

        public DataExportService(IWindowService windowService, TabControlEditViewModel tabControlEditViewModel, ILogger logger)
        {
            _windowService = windowService;
            _tabControlEditViewModel = tabControlEditViewModel;
            _logger = logger;
        }


    }
}
