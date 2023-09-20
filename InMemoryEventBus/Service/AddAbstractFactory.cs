
using System;
using Microsoft.Extensions.DependencyInjection;

namespace InMemoryEventBus.Service
{
    public interface IAbstractFactory<out T>
    {
        T Create();
    }

    public class AbstractFactory<T> : IAbstractFactory<T>
    {
        private readonly Func<T> _factory;

        public AbstractFactory(Func<T> factory)
        {
            _factory = factory;
        }

        public T Create() => _factory();
    }
    public static class AddAbstractFactoryExtensions
    {
        public static void AddAbtractFactort<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        {
            services.AddTransient<TInterface, TImplementation>();
            services.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
            services.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
        }

    }
}
