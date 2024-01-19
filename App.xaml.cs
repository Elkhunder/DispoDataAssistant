﻿using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Data.Models.Settings;
using DispoDataAssistant.Extensions.Service;
using DispoDataAssistant.Helpers;
using DispoDataAssistant.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    public App()
    {
        ConfigureSerilog();
        AppHost = Host.CreateDefaultBuilder()
                      .ConfigureServices((hostContext, services) =>
                      {
                          services.AddViews();
                          services.AddViewModels();
                          services.AddInternalServices();


                          services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
                          services.AddDbContext<ServiceNowAssetContext>();
                          services.AddDbContext<SettingsContext>();
                      })
                      .Build();


        //this.InitializeComponent();




    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        MainWindow startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        IUserSettingsService _userSettingsService = AppHost.Services.GetRequiredService<IUserSettingsService>();
        ISettingsService _settingsService = AppHost.Services.GetRequiredService<ISettingsService>();

        System.Collections.Generic.Dictionary<string, object> userSettings = _settingsService.GetAllUserSettings();
        _userSettingsService.ApplyUserSettings(userSettings);

        using (IServiceScope scope = AppHost.Services.CreateScope())
        {
            using (ServiceNowAssetContext dbContext = scope.ServiceProvider.GetRequiredService<ServiceNowAssetContext>())
            {
                dbContext.Database.Migrate();
                Task.Run(async () => await PopulateDatabase(dbContext)).Wait();
            }
            using (SettingsContext dbContext = scope.ServiceProvider.GetRequiredService<SettingsContext>())
            {
                dbContext.Database.Migrate();
                Task.Run(async () => await InitializeSettings(dbContext)).Wait();

                Task.Run(async () => await LoadSettings(dbContext)).Wait();
            }
        }


        startupForm.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        App.Current.Shutdown();
    }

    private static async Task InitializeSettings(SettingsContext context)
    {
        if (await context.Settings.AnyAsync())
        {
            return;
        }
        IntegrationTypes integrationsTypes = new();
        GeneralTypes generalSettingTypes = new();
        Settings settings = new() { Title = "Application Settings" };
        List<Integration> integrations = [];
        List<General> generalSettings = [];

        foreach (string setting in generalSettingTypes)
        {
            //await context.General.AddAsync(new GeneralSetting { Title = setting, Value = "20", SettingsModel = settingsModel });
            generalSettings.Add(new General { Title = setting, Value = "20", Settings = settings });
            //await context.SaveChangesAsync(true);
        }

        foreach (string integration in integrationsTypes)
        {
            //await context.Integrations.AddAsync(new Integration { Title = integration, IsEnabled = false, SettingsModel = settingsModel });
            integrations.Add(new Integration { Title = integration, IsEnabled = true, Settings = settings });
            //await context.SaveChangesAsync(true);
        }
        settings.General = generalSettings;
        settings.Integrations = integrations;

        context.Integrations.AddRange(integrations);
        context.General.AddRange(generalSettings);
        context.Settings.Add(settings);

        context.SaveChanges();

    }

    private static async Task LoadSettings(SettingsContext context)
    {
        List<Settings> settings = await context.Settings.ToListAsync();
    }

    private static async Task PopulateDatabase(ServiceNowAssetContext context)
    {
        if (await context.ServiceNowAssets.AnyAsync())
        {
            return;
        }

        foreach (ServiceNowAsset a in ServiceNowAssetDataGenerator.GenerateData(50))
        {
            context.ServiceNowAssets.Add(new ServiceNowAsset()
            {
                SysId = a.SysId,
                AssetTag = a.AssetTag,
                SerialNumber = a.SerialNumber,
                Model = a.Model,
                Manufacturer = a.Manufacturer,
                Category = a.Category,
                InstallStatus = a.InstallStatus,
                LastUpdated = a.LastUpdated,
                OperationalStatus = a.OperationalStatus,
            });
        }
        context.SaveChanges();
    }

    private static void ConfigureSerilog()
    {
        string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DispoDataAssistant", "log.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }






















































    //public new static App Current => (App)Application.Current;

    //public IServiceProvider? Services { get; }
    //private static IServiceProvider ConfigureServices()
    //{
    //    IServiceCollection services = new ServiceCollection();

    //    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    //    services.AddDbContext<ServiceNowAssetContext>(
    //        options =>
    //        {
    //            options.UseSqlite("Data Source=DispoAssisstant.db");
    //            options.UseLazyLoadingProxies();
    //        });

    //    services.AddSingleton<DeviceDetails>();
    //    services.AddSingleton<DeviceInformation>();
    //    services.AddSingleton<Themes>();

    //    // Services
    //    services.AddSingleton<ISettingsService, SettingsManager>();
    //    services.AddSingleton<IThemeService, ThemeService>();
    //    services.AddSingleton<IUserSettingsService, UserSettingsService>();
    //    services.AddSingleton<IDataInputService, DataInputService>();
    //    services.AddSingleton<IWindowService, WindowService>();
    //    services.AddSingleton<ITabManager, TabManager>();

    //    //Factories
    //    services.AddSingleton<TabItemFactory>();

    //    // View Models
    //    services.AddSingleton<SettingsViewModel>();
    //    services.AddSingleton<WindowControlViewModel>();
    //    services.AddSingleton<DataInputViewModel>();
    //    services.AddSingleton<DataActionsViewModel>();
    //    services.AddSingleton<ViewPaneViewModel>();
    //    services.AddSingleton<MainViewModel>();
    //    services.AddSingleton<TabControlEditViewModel>();
    //    services.AddSingleton<TabControlButtonsViewModel>();

    //    services.AddTransient<MainWindow>();
    //    services.AddTransient<TabControlEditWindowView>();

    //    return services.BuildServiceProvider();
    //}
}
