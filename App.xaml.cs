using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using DispoDataAssistant.ViewModels;
using DispoDataAssistant.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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
            ConfigureSerilog();
            IServiceProvider serviceProvider = ConfigureServices();
            Ioc.Default.ConfigureServices(serviceProvider);

            IUserSettingsSerivce _userSettingsSerivce = Ioc.Default.GetRequiredService<IUserSettingsSerivce>();
            ISettingsService _settingsService = Ioc.Default.GetRequiredService<ISettingsService>();
            DataInputViewModel _dataInputViewModel = Ioc.Default.GetRequiredService<DataInputViewModel>();
            Dictionary<string, object> userSettings = _settingsService.GetAllUserSettings();


            this.InitializeComponent();

            _userSettingsSerivce.ApplyUserSettings(userSettings);
        }

        private void ConfigureSerilog()
        {
            var logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DispoDataAssistant", "log.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider? Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
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
