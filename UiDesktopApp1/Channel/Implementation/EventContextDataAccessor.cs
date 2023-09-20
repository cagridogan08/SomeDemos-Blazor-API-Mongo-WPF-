using System.Threading;
using System.Windows.Controls;

namespace UiDesktopApp1.Channel.Implementation
{
    internal class EventContextDataAccessor<T>:IEventContextAccessor<T>
    {
        private static readonly AsyncLocal<EventMetadataWrapper<T>> Holder = new();

        public Event<T>? Event => Holder.Value?.Event;
        public void Set(Event<T> @event)
        {
           var holder = Holder.Value;
           if (holder is not null)
           {
               holder.Event = null;
           }
           Holder.Value = new EventMetadataWrapper<T> { Event = @event};
        }
    }

    internal class EventMetadataWrapper<T>
    {
        public Event<T>? Event { get; set; }
    }
}
