using System.Threading;
using System.Threading.Tasks;

namespace InMemoryEventBus.Handlers
{
    public interface IEventHandler<in T>
    {
        ValueTask HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
