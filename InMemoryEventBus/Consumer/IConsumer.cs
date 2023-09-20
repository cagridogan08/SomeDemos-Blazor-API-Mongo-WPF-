
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InMemoryEventBus.Consumer
{
    public interface IConsumer : IAsyncDisposable
    {
        ValueTask StartAsync(CancellationToken token = default);
        ValueTask StopAsync(CancellationToken token = default);
    }

    public interface IConsumer<T> : IConsumer
    {
    }
}
