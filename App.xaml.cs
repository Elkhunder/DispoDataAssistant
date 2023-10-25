using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using DispoDataAssistant.ViewModels;
using DispoDataAssistant.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace DispoDataAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserSettingsService _userSettingsSerivce;
        public App()
        {
            IServiceProvider serviceProvider = ConfigureServices();
            Ioc.Default.ConfigureServices(serviceProvider);

            this.InitializeComponent();

            IUserSettingsService _userSettingsSerivce = Ioc.Default.GetRequiredService<IUserSettingsService>();
            ISettingsService _settingsService = Ioc.Default.GetRequiredService<ISettingsService>();

            Dictionary<string, object> userSettings = _settingsService.GetAllUserSettings();
            _userSettingsSerivce.ApplyUserSettings(userSettings);
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider? Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            // Add the ServiceNowApiClient as a scoped service
            services.AddScoped<IServiceNowApiClient>(provider =>
            {
                var baseUrl = ConfigurationManager.AppSettings["ServiceNowBaseUrl"]; // Read the base URL from configuration

                if (baseUrl != null)
                {
                    return new ServiceNowApiClient(baseUrl);
                }
                else
                {
                    return new ServiceNowApiClient("https://ummeddev.service-now.com/api/now/");
                }
            });

            services.AddSingleton<DeviceDetails>();
            services.AddSingleton<DeviceInformation>();
            services.AddSingleton<Themes>();
            services.AddSingleton<ISettingsService, SettingsManager>();
            services.AddSingleton<IThemeService, ThemeManager>();
            services.AddSingleton<IUserSettingsService, UserSettingsManager>();
            services.AddSingleton<IDataInputService, DataInputManager>();
            services.AddSingleton<IWindowService, WindowManager>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<WindowControlViewModel>();
            services.AddSingleton<DataInputViewModel>();
            services.AddSingleton<DataActionsViewModel>();
            //services.AddSingleton<ViewModelLocator>();

            return services.BuildServiceProvider();
        }
    }
}
