using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using DomainLibrary;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache
{
    public class ServiceSyncCollection<T> : ObservableCollection<T> where T : Entity
    {
        private readonly IDataService<T> _service;

        public ServiceSyncCollection(IDataService<T> service)
        {
            _service = service;
            Task.Run(async () =>
            {
                var items = await _service.GetData();
                foreach (var item in items)
                {
                    Add(item);
                }
            });
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

        protected override void SetItem(int index, T item)
        {
            _service.UpdateData(item);
            base.SetItem(index, item);
        }

        #endregion
    }
}
