using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using InMemoryEventBus.Models;

namespace InMemoryEventBus.Publisher
{
    public class EventPublisher<T> : IProducer<T>
    {
        private readonly ChannelWriter<Event<T>> _writer;

        public EventPublisher(ChannelWriter<Event<T>> writer)
        {
            _writer = writer;
        }

        public async ValueTask PublishAsync(Event<T> @event, CancellationToken token = default)
        {
            await _writer.WriteAsync(@event, token).ConfigureAwait(false);
        }

        public ValueTask DisposeAsync()
        {
            _writer.TryComplete();
            return ValueTask.CompletedTask;
        }
    }
}
