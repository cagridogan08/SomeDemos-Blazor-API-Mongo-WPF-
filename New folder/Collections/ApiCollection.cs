using System;
using System.Collections.Generic;
using Iotech.Link.Libs.Core.Base.Data.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfAppWithRedisCache.Extensions;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache.Collections
{
    public class ApiCollection<T> : ObservableCollection<T> where T : EntityBase
    {
        #region Constructors

        private readonly IHttpClientService _service;
        private readonly TaskCompletionSource<bool> _initCompletionSource = new();

        public ApiCollection(IHttpClientService service)
        {
            _service = service;
            _service.Get<T>().Await(AddRange);
        }

        private void AddRange(ICollection<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }

            try
            {
                _initCompletionSource.SetResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Methods

        protected override async void InsertItem(int index, T item)
        {
            await _initCompletionSource.Task;
            if (this.All(e => e.Name != item.Name))
            {
                _service.Create(new List<T>() { item }).Await((val) =>
                {
                    if (val)
                    {
                        base.InsertItem(index, item);
                    }
                });
            }

        }

        protected override void RemoveItem(int index)
        {
            var itm = this[index];
            _service.Delete(new List<T>() { itm }).Await((val) =>
            {
                if (val)
                {
                    base.RemoveItem(index);
                }
            });
        }

        protected override void SetItem(int index, T item)
        {
            _service.Update(new List<T>() { item }).Await((val) =>
            {
                if (val)
                {
                    base.SetItem(index, item);
                }
            });
        }

        #endregion
    }
}
