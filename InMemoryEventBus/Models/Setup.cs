using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using InMemoryEventBus.Consumer;
using InMemoryEventBus.Handlers;
using InMemoryEventBus.Publisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InMemoryEventBus.Models
{
    public static class Setup
    {
        public static IServiceCollection AddInMemoryEventBus<T, THandler>(this IServiceCollection services) where THandler : class, IEventHandler<T>
        {
            var bus = Channel.CreateUnbounded<Event<T>>(new UnboundedChannelOptions() { AllowSynchronousContinuations = false });

            services.AddScoped<IEventHandler<T>, THandler>();

            services.AddSingleton(typeof(IProducer<T>), _ => new EventPublisher<T>(bus.Writer));

            var consumerFactory = (IServiceProvider provider) => new EventConsumer<T>(bus.Reader, provider.GetRequiredService<IServiceScopeFactory>(), provider.GetRequiredService<ILoggerFactory>().CreateLogger<EventConsumer<T>>());

            services.AddSingleton(typeof(IConsumer), consumerFactory.Invoke);
            services.AddSingleton(typeof(IConsumer<T>), consumerFactory.Invoke);

            services.AddSingleton(typeof(IEventContextAccessor<T>), typeof(EventContextAccessor<T>));
            return services;
        }

        public static async Task<IServiceProvider> StartConsumers(this IServiceProvider provider)
        {
            var consumers = provider.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.StartAsync().ConfigureAwait(false);
            }

            return provider;
        }

        public static async Task<IServiceProvider> StopConsumers(this IServiceProvider provider)
        {
            var consumers = provider.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.StopAsync().ConfigureAwait(false);
            }

            return provider;
        }
    }
}
