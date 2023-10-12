using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => Ioc.Default.GetService<MainViewModel>();
        public SettingsViewModel SettingsViewModel = Ioc.Default.GetService<SettingsViewModel>();
        public WindowControlViewModel WindowControlViewModel = Ioc.Default.GetService<WindowControlViewModel>();
        public DataActionsViewModel DataActionsViewModel = Ioc.Default.GetService<DataActionsViewModel>();
        public DataInputViewModel DataInputViewModel = Ioc.Default.GetService<DataInputViewModel>();
    }
}
