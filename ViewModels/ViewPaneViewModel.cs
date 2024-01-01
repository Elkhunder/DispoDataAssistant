using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DispoDataAssistant.ViewModels
{
    public partial class ViewPaneViewModel : BaseViewModel
    {
        private readonly ITabManager? _tabManager;

        public ViewPaneViewModel(ILogger<ViewPaneViewModel> logger, ITabManager tabManager) : base(logger)
        {
            _tabManager = tabManager;
        }

        
    }
}
