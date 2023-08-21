using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManagerLibrary.Managers.Abstract;
using DomainLibrary;

namespace WpfAppWithRedisCache.Services
{
    internal class ProductDataService : IDataService<Product>
    {
        private readonly IEntityManager<Product> _context;
        private readonly ICacheService _cacheService;

        public ProductDataService(IEntityManager<Product> context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }


        public async Task<ICollection<Product>> GetData()
        {
            var cacheData = _cacheService.GetData<ICollection<Product>>($"{nameof(Product)}s");
            if (cacheData is not null && cacheData.Any()) return cacheData;
            var data = await _context.GetAllAsync();
            _cacheService.SetData($"{nameof(Product)}s", data, DateTimeOffset.Now.AddMinutes(5));
            return data;
        }

        public async Task<Product?> GetDataOrNull(int id)
        {
            var cacheData = _cacheService.GetData<Product>($"{nameof(Product)}{id}");
            if (cacheData is not null) return cacheData;
            var data = await _context.GetAsync(id);
            _cacheService.SetData($"{nameof(Product)}{id}", data, DateTimeOffset.Now.AddMinutes(5));
            return data;
        }

        public async Task<bool> RemoveData(int id)
        {
            if (!await _context.DeleteAsync(id)) return false;
            _cacheService.RemoveData($"product{id}");
            return true;
        }

        public async Task<bool> AddData(Product data)
        {
            if (await _context.AddAsync(data))
            {
                _cacheService.SetData($"{nameof(Product)}{data.Id}", data, DateTimeOffset.Now.AddMinutes(5));
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateData(Product data)
        {
            if (await _context.UpdateAsync(data))
            {
                _cacheService.SetData($"{nameof(Product)}{data.Id}", data, DateTimeOffset.Now.AddMinutes(5));
                return true;
            }

            return false;
        }
    }
}
