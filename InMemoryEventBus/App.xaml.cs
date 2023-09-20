using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using InMemoryEventBus.Handlers;
using InMemoryEventBus.Models;
using InMemoryEventBus.ViewModels;

namespace InMemoryEventBus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly IHost _applicationHost;

        public App()
        {
            _applicationHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging(((context, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                    builder.AddConfiguration(context.Configuration);
                }))
                .ConfigureServices((_, services) =>
                {
                    services.AddInMemoryEventBus<int, OrderNumberEventHandler>();
                    services.AddInMemoryEventBus<OrderEvent, OrderPlacedEventHandler>();
                    services.AddInMemoryEventBus<OrderEvent, TrackUserOrderItemsEventHandler>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _applicationHost.StartAsync();
            await _applicationHost.Services.StartConsumers();
            GetService<MainWindow>()?.Show();

        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await _applicationHost.StopAsync();
            await _applicationHost.Services.StopConsumers();
            _applicationHost.Dispose();
        }
        public T? GetService<T>()
        {
            return _applicationHost.Services.GetService<T>();
        }
    }
}
