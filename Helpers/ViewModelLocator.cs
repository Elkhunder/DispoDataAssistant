using DispoDataAssistant.UIComponents.DataInput;
using DispoDataAssistant.UIComponents.Main;
using DispoDataAssistant.UIComponents.Settings;
using DispoDataAssistant.UIComponents;

namespace DispoDataAssistant.Helpers
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel;
        public SettingsViewModel SettingsViewModel;
        public DataInputViewModel DataInputViewModel;
        public TabControlButtonsViewModel TabControlButtonsViewModel;
        public TabControlEditViewModel TabControlEditViewModel;

        public ViewModelLocator(
            MainViewModel mainViewModel, 
            SettingsViewModel settingsViewModel,
            DataInputViewModel dataInputViewModel, 
            TabControlButtonsViewModel tabControlButtonsViewModel,
            TabControlEditViewModel tabControlEditViewModel
            )
        {
            MainViewModel = mainViewModel;
            SettingsViewModel = settingsViewModel;
            DataInputViewModel = dataInputViewModel;
            TabControlButtonsViewModel = tabControlButtonsViewModel;
            TabControlEditViewModel = tabControlEditViewModel;
        }
    }
}
