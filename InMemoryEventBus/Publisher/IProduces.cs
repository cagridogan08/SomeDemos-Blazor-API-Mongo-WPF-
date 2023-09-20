
using System;
using System.Threading;
using System.Threading.Tasks;
using InMemoryEventBus.Models;

namespace InMemoryEventBus.Publisher
{
    public interface IProducer<T> : IAsyncDisposable
    {
        ValueTask PublishAsync(Event<T> @event, CancellationToken token = default);
    }
}
