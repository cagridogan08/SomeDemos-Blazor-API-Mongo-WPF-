using System;
using System.Threading.Tasks;
using InMemoryEventBus.Models;
using InMemoryEventBus.Publisher;

namespace InMemoryEventBus.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructor

        private readonly IProducer<int> _producer;

        public MainViewModel(IProducer<int> producer, IServiceProvider provider)
        {
            _producer = producer;
            var hh = provider;
        }

        #endregion

        public async Task PublishAsync(int val)
        {
            await _producer.PublishAsync(new Event<int>(val));
        }
    }
}
