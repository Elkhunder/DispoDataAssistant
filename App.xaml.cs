using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using DispoDataAssistant.ViewModels;
using DispoDataAssistant.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DispoDataAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ViewModelLocator Locator => Ioc.Default.GetService<ViewModelLocator>();
        public App()
        {
            IServiceProvider serviceProvider = ConfigureServices();
            Ioc.Default.ConfigureServices(serviceProvider);

            IUserSettingsSerivce _userSettingsSerivce = Ioc.Default.GetService<IUserSettingsSerivce>() ?? throw new InvalidOperationException("IUserSettingsService not registered.");
            ISettingsService _settingsService = Ioc.Default.GetService<ISettingsService>() ?? throw new InvalidOperationException("ISettingsService not registered.");
            DataInputViewModel _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>() ?? throw new InvalidOperationException("DataInputViewModel not registered");
            Dictionary<string, object> userSettings = _settingsService.GetAllUserSettings();


            this.InitializeComponent();

            _userSettingsSerivce.ApplyUserSettings(userSettings);
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider? Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DeviceDetails>();
            services.AddSingleton<DeviceInformation>();
            services.AddSingleton<Themes>();
            services.AddSingleton<ISettingsService, SettingsManager>();
            services.AddSingleton<IThemesService, ThemesManager>();
            services.AddSingleton<IUserSettingsSerivce, UserSettingsManager>();
            services.AddSingleton<IDataInputService, DataInputManager>();
            services.AddSingleton<IWindowService, WindowManager>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<WindowControlViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DataInputViewModel>();
            services.AddSingleton<DataActionsViewModel>();
            services.AddSingleton<ViewModelLocator>();

            return services.BuildServiceProvider();
        }
    }
}
