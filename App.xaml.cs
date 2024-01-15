using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Extensions;
using DispoDataAssistant.Helpers;
using DispoDataAssistant.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
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
                          services.AddDbContext<ServiceNowAssetContext>(
                              options =>
                              {
                                  options.UseSqlite("Data Source=DispoAssisstant.db");
                                  options.UseLazyLoadingProxies();
                              });
                      })
                      .Build();
        

        //this.InitializeComponent();

        

        
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        var _userSettingsService = AppHost.Services.GetRequiredService<IUserSettingsService>();
        var _settingsService = AppHost.Services.GetRequiredService<ISettingsService>();

        var userSettings = _settingsService.GetAllUserSettings();
        _userSettingsService.ApplyUserSettings(userSettings);

        using (var scope = AppHost.Services.CreateScope())
        using (var dbContext = scope.ServiceProvider.GetRequiredService<ServiceNowAssetContext>())
        {
            dbContext.Database.EnsureCreated();
            Task.Run(async () => await PopulateDatabase(dbContext)).Wait();
        }
        
        startupForm.Show();

        base.OnStartup(e);
    }

    private static async Task PopulateDatabase(ServiceNowAssetContext context)
    {
        if(await context.ServiceNowAssets.AnyAsync())
        {
            return;
        }

        foreach (var a in ServiceNowAssetDataGenerator.GenerateData(50))
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
        var logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DispoDataAssistant", "log.txt");

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
