using System;
using System.Collections.Generic;
using System.Linq;
using WpfAppWithRedisCache.Context;
using WpfAppWithRedisCache.Models;

namespace WpfAppWithRedisCache.Services
{
    internal class ProductDataService : IDataService<Product>
    {
        private readonly ApplicationDataContext _context;
        private readonly ICacheService _cacheService;

        public ProductDataService(ApplicationDataContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public IDataService<Product> GetDataService()
        {
            return this;
        }

        public ICollection<Product> GetData()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("products");
            if (cacheData is not null)
            {
                return cacheData.ToList();
            }

            var cacheTimeout = DateTimeOffset.Now.AddMinutes(2.0);
            cacheData = _context.Products.ToList();
            _cacheService.SetData("products", cacheData, cacheTimeout);
            foreach (var product in cacheData)
            {
                _cacheService.SetData($"product{product.Id}", product, cacheTimeout);
            }
            return cacheData.ToList();
        }

        public Product? GetDataOrNull(int id)
        {
            var cacheData = _cacheService.GetData<Product>($"product{id}");
            if (cacheData is not null)
            {
                return cacheData;
            }
            var cacheTimeout = DateTimeOffset.Now.AddMinutes(2.0);

            cacheData = _context.Products.FirstOrDefault(itm => itm.Id == id);
            if (cacheData != null) _cacheService.SetData<Product>($"product{id}", cacheData, cacheTimeout);
            return cacheData;
        }

        public bool RemoveData(int id)
        {
            var contextData = _context.Products.FirstOrDefault(e => e.Id == id);
            if (contextData != null)
            {
                _context.Products.Remove(contextData);
                _cacheService.RemoveData($"product{contextData.Id}");
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool AddData(Product data)
        {
            if (_context.Products.FirstOrDefault(e => e.Name == data.Name) is null)
            {
                var obj = _context.Products.Add(data);
                _context.SaveChanges();
                _cacheService.SetData($"product{data.Id}", obj.Entity, DateTimeOffset.Now.AddMinutes(2));
                return true;
            }
            return false;
        }

        public bool UpdateData(Product data)
        {
            return true;
        }
    }
}
