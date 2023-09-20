using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using DataManagerLibrary.Context;
using DataManagerLibrary.Managers;
using DataManagerLibrary.Managers.Abstract;
using DomainLibrary;
using Iotech.Link.Libs.Modules.LinkRestAPI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WpfAppWithRedisCache.Collections;
using WpfAppWithRedisCache.Services;
using WpfAppWithRedisCache.ViewModels;
using WpfAppWithRedisCache.Views;

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
                            .GetConnectionString("DefaultConnection")!);
                    }));
                    services.AddScoped<IEntityManager<Product>, ProductManager>();
                    services.AddScoped<IAuthenticationService, AuthenticationService>((_ =>
                        new AuthenticationService(new HttpClient() { BaseAddress = new Uri("http://localhost:5001/") })));
                    services.AddSingleton<AuthTokenHandler>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:5001/");
                    }).AddHttpMessageHandler<AuthTokenHandler>();
                    services.AddSingleton(_ =>
                    {
                        var hubConnection = new HubConnectionBuilder()
                            .WithUrl("http://localhost:5041/messages")
                            .Build();
                        return hubConnection;
                    });
                    services.AddScoped<ICacheService, CacheService>();
                    services.AddScoped<IDataService<Product>, ProductDataService>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<Window1>();
                })
                .Build();
            Configuration = _applicationHost.Services.GetRequiredService<IConfiguration>();
            _applicationHost.Services.GetRequiredService<ILoggerFactory>().AddSerilog();
            var items = _applicationHost.Services.GetRequiredService<IHttpClientService>().Get<CompType>().Result;
            var hh = new ApiCollection<CompType>(_applicationHost.Services.GetRequiredService<IHttpClientService>());
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
            GetRequiredService<Window1>()?.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _applicationHost.Services.GetRequiredService<IHttpClientService>().HttpClient
                 .DeleteAsync($"api/ConfSession?description=Migration_{DateTime.Now.ToString("O")}");
            await _applicationHost.StopAsync();
            await GetRequiredService<ApplicationDataContext>()?.Database?.CloseConnectionAsync()!;
            await GetRequiredService<HubConnection>()!.DisposeAsync();
            _applicationHost.Dispose();
        }
    }
}
