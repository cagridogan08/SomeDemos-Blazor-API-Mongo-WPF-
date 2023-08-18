using System;
using System.Threading.Tasks;
using System.Windows;
using DomainLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WpfAppWithRedisCache.Context;
using WpfAppWithRedisCache.Services;
using WpfAppWithRedisCache.ViewModels;

namespace WpfAppWithRedisCache
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IConfiguration Configuration { get; set; }
        private readonly IHost _applicationHost;
        public App()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().Enrich.FromLogContext().WriteTo.File("Test.log").CreateLogger();

            _applicationHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging(((context, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(context.Configuration);
                }))
                .ConfigureServices((_, services) =>
                {
                    services.AddDbContext<ApplicationDataContext>(((provider, builder) =>
                    {
                        builder.UseNpgsql(provider.GetRequiredService<IConfiguration>()
                            .GetConnectionString("DefaultConnection"));
                    }));
                    services.AddScoped<ICacheService, CacheService>();
                    services.AddScoped<IDataService<Product>, ProductDataService>();
                    services.AddScoped<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
            Configuration = _applicationHost.Services.GetRequiredService<IConfiguration>();
        }

        public T? GetRequiredService<T>() where T : notnull
        {
            return ((App)Current)._applicationHost.Services.GetService<T>();
        }


        public static string? GetConfiguration(string key)
        {
            return ((App)Current).Configuration[key];
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _applicationHost.StartAsync();
            var dataContext = GetRequiredService<ApplicationDataContext>();
            if (dataContext != null)
            {
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            if (await dataContext.Database.CanConnectAsync())
                            {
                                await dataContext.Database.EnsureCreatedAsync();
                                break; // Connection is established, exit the loop
                            }
                        }
                        catch (Exception)
                        {
                            // Handle the exception if needed
                            // You might want to introduce a delay here before trying again
                        }
                    }
                });
            }
            GetRequiredService<MainWindow>()?.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _applicationHost.StopAsync();
            _applicationHost.Dispose();
        }
    }
}
