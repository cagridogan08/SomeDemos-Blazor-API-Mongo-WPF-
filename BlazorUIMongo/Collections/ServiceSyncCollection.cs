using System.Collections.ObjectModel;
using BlazorUIMongo.Services;

namespace BlazorUIMongo.Collections
{
    public class ServiceSyncCollection<T> : ObservableCollection<T>
    {
        private readonly IService<T> _service;

        public ServiceSyncCollection(IService<T> service)
        {
            _service = service;
            _service.Get().
                Await(items =>
                    {
                        foreach (var item in items)
                        {
                            Add(item);
                        }
                    }
                );
        }

        #region Methods
        protected override void InsertItem(int index, T item)
        {
            _service.Add(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var itm = this[index];
            _service.Delete(itm);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            //_service.UpdateData(item);
            base.SetItem(index, item);
        }

        #endregion
    }
}
