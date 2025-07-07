namespace DispoDataAssistant.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel;
        public SettingsViewModel SettingsViewModel;
        public WindowControlViewModel WindowControlViewModel;
        public DataActionsViewModel DataActionsViewModel;
        public DataInputViewModel DataInputViewModel;

        public ViewModelLocator(MainViewModel mainViewModel, SettingsViewModel settingsViewModel, WindowControlViewModel windowControlViewModel, DataActionsViewModel dataActionsViewModel, DataInputViewModel dataInputViewModel)
        {
            MainViewModel = mainViewModel;
            SettingsViewModel = settingsViewModel;
            WindowControlViewModel = windowControlViewModel;
            DataActionsViewModel = dataActionsViewModel;
            DataInputViewModel = dataInputViewModel;
        }
    }
}
