
using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UiDesktopApp1.Channel.Implementation;

namespace UiDesktopApp1.Channel
{
    public static class Setup
    {
        public static IServiceCollection AddInMemoryEvent<T, THandler>(this IServiceCollection services)
            where THandler : class, IEventHandler<T>
        {
            var bus = System.Threading.Channels.Channel.CreateUnbounded<Event<T>>((new UnboundedChannelOptions(){AllowSynchronousContinuations = false}));

            services.AddScoped<IEventHandler<T>, THandler>();

            services.AddSingleton(typeof(IProducer<T>), _ => new EventPublisher<T>(bus.Writer));

            var consumerFactory = (IServiceProvider provider) => new InMemoryEventConsumer<T>(
                bus.Reader,
                provider.GetRequiredService<IServiceScopeFactory>(),
                provider.GetRequiredService<ILoggerFactory>().CreateLogger<InMemoryEventConsumer<T>>()
            );

            services.AddSingleton(typeof(IConsumer), consumerFactory.Invoke);
            services.AddSingleton(typeof(IConsumer<T>), consumerFactory.Invoke);

            services.AddSingleton(typeof(IEventContextAccessor<T>), typeof(EventContextDataAccessor<T>));

            return services;
        }

        public static async Task<IServiceProvider> StartConsumers(this IServiceProvider provider)
        {
            var consumers = provider.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.Start().ConfigureAwait(false);
            }

            return provider;
        }

        public static async Task<IServiceProvider> StopConsumers(this IServiceProvider provider)
        {
            var consumers = provider.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.Stop().ConfigureAwait(false);
            }

            return provider;
        }
    }
}
