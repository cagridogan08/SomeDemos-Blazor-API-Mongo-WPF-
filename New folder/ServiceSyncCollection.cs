using System.Collections.ObjectModel;
using WpfAppWithRedisCache.Models;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache
{
    public class ServiceSyncCollection<T> : ObservableCollection<T> where T : Entity
    {
        private readonly IDataService<T> _service;

        public ServiceSyncCollection(IDataService<T> service)
        {
            _service = service;
        }

        #region Methods

        protected override void InsertItem(int index, T item)
        {
            _service.AddData(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var itm = this[index];
            _service.RemoveData(itm.Id);
            base.RemoveItem(index);
        }

        #endregion
    }
}
