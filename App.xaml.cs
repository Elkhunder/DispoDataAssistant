using DispoDataAssistant.Data.Contexts;
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
using System.Linq;
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
                          services.AddClients();


                          services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
                          services.AddDbContext<AssetContext>();
                          services.AddDbContext<SettingsContext>();
                      })
                      .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        bool isDebugMode = false;
        #if DEBUG
        isDebugMode = true;
        #endif

        await AppHost!.StartAsync();

        using (IServiceScope scope = AppHost.Services.CreateScope())
        {
            using (AssetContext context = scope.ServiceProvider.GetRequiredService<AssetContext>())
            {
                context.Database.Migrate();
                if(isDebugMode)
                {
                    PopulateDatabase(context);
                }
                else if (!context.Tabs.Any())
                {
                    context.Tabs.Add(new TabModel() { Name = "Default", ServiceNowAssets = [] });
                    context.SaveChanges();
                }
            }
            using (SettingsContext dbContext = scope.ServiceProvider.GetRequiredService<SettingsContext>())
            {
                dbContext.Database.Migrate();
                InitializeSettings(dbContext);
                LoadSettings(dbContext);
            }
        }

        MainWindow startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        IUserSettingsService _userSettingsService = AppHost.Services.GetRequiredService<IUserSettingsService>();
        ISettingsService _settingsService = AppHost.Services.GetRequiredService<ISettingsService>();

        System.Collections.Generic.Dictionary<string, object> userSettings = _settingsService.GetAllUserSettings();
        _userSettingsService.ApplyUserSettings(userSettings);

        startupForm.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        App.Current.Shutdown();
    }

    private static void InitializeSettings(SettingsContext context)
    {
        if (context.Settings.Any())
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
        }

        foreach (string integration in integrationsTypes)
        {
            //await context.Integrations.AddAsync(new Integration { Title = integration, IsEnabled = false, SettingsModel = settingsModel });
            integrations.Add(new Integration { Title = integration, IsEnabled = true, Settings = settings });
        }
        settings.General = generalSettings;
        settings.Integrations = integrations;

        context.Integrations.AddRange(integrations);
        context.General.AddRange(generalSettings);
        context.Settings.Add(settings);

        context.SaveChanges();

    }

    private static void LoadSettings(SettingsContext context)
    {
        List<Settings> settings = [.. context.Settings];
    }

    private static void PopulateDatabase(AssetContext context)
    {
        if (context.ServiceNowAssets.Any())
        {
            return;
        }
        var newTab1 = new TabModel() { Name = "December" };
        var newTab2 = new TabModel() { Name = "January" };

        context.Tabs.AddRange([newTab1, newTab2]);
        context.SaveChanges();

        var tab1 = context.Tabs.Single(e => e.Name == "December");
        var tab2 = context.Tabs.Single(e => e.Name == "January");


        foreach (ServiceNowAsset a in ServiceNowAssetDataGenerator.GenerateData(50, tab1))
        {
            
            context.ServiceNowAssets.Add(new ServiceNowAsset()
            {
                SysId = a.SysId,
                AssetTag = a.AssetTag,
                SerialNumber = a.SerialNumber,
                Model = a.Model,
                Manufacturer = a.Manufacturer,
                Category = a.Category,
                State = a.State,
                LastUpdated = a.LastUpdated,
                Substate = a.Substate,
                LifeCycleStage = a.LifeCycleStage,
                LifeCycleStatus = a.LifeCycleStatus,
                Parent = a.Parent,
                Tab = a.Tab,
            });
        }

        foreach (ServiceNowAsset a in ServiceNowAssetDataGenerator.GenerateData(50, tab2))
        {
            context.ServiceNowAssets.Add(new ServiceNowAsset()
            {
                SysId = a.SysId,
                AssetTag = a.AssetTag,
                SerialNumber = a.SerialNumber,
                Model = a.Model,
                Manufacturer = a.Manufacturer,
                Category = a.Category,
                State = a.State,
                LastUpdated = a.LastUpdated,
                Substate = a.Substate,
                LifeCycleStage = a.LifeCycleStage,
                LifeCycleStatus = a.LifeCycleStatus,
                Parent = a.Parent,
                Tab = a.Tab,
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
}
