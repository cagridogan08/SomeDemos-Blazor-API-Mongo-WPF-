using System.Threading;

namespace InMemoryEventBus.Models
{
    public class EventContextAccessor<T> : IEventContextAccessor<T>
    {
        private static readonly AsyncLocal<EventMetadataWrapper<T>> EventContext = new();
        public Event<T>? Event { get; }
        public void Set(Event<T> @event)
        {
            var holder = EventContext.Value;
            if (holder is not null)
            {
                holder.Event = null;
            }
            EventContext.Value = new EventMetadataWrapper<T>() { Event = @event };
        }
    }
    internal sealed class EventMetadataWrapper<T>
    {
        public Event<T>? Event { get; set; }
    }
}
