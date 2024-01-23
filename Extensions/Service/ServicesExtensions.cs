﻿using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Factories;
using DispoDataAssistant.Managers.Implementations;
using DispoDataAssistant.Managers.Interfaces;
using DispoDataAssistant.Services.Implementations;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents;
using DispoDataAssistant.UIComponents.DataInput;
using DispoDataAssistant.UIComponents.Main;
using DispoDataAssistant.UIComponents.Settings;
using DispoDataAssistant.UIComponents.ViewPane;
using Microsoft.Extensions.DependencyInjection;

namespace DispoDataAssistant.Extensions.Service
{
    public static class ServicesExtensions
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<DataInputViewModel>();
            services.AddSingleton<ViewPaneViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TabControlEditViewModel>();
            services.AddSingleton<TabControlButtonsViewModel>();
        }

        public static void AddInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<DeviceDetails>();
            services.AddSingleton<DeviceInformation>();
            services.AddSingleton<Themes>();

            // Services
            services.AddSingleton<ISettingsService, SettingsManager>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IUserSettingsService, UserSettingsService>();
            services.AddSingleton<IDataInputService, DataInputService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<ITabManager, TabManager>();

            //Factories
            services.AddSingleton<TabItemFactory>();

        }

        public static void AddViews(this IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            //services.AddTransient<MainView>();
            services.AddTransient<SettingsMenuView>();
            services.AddTransient<TabControlEditWindowView>();
            services.AddTransient<SettingsView>();
        }
    }
}