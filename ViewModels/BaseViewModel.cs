using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        protected readonly ILogger? _logger;
        public BaseViewModel(ILogger? logger)
        {
            _logger = logger;
            PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"Property changed: {e.PropertyName}");
            };
        }
    }
}
