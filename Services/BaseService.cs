﻿using CommunityToolkit.Mvvm.ComponentModel;
using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DispoDataAssistant.Services
{
    public class BaseService : ObservableObject
    {
        protected readonly ILogger _logger;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }
    }
}
