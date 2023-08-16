using WpfAppWithRedisCache.Models;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataService<Product> _service;
        private ServiceSyncCollection<Product> _products;

        public MainViewModel(IDataService<Product> service)
        {
            _service = service;
            _products = new ServiceSyncCollection<Product>(_service);
        }

        public ServiceSyncCollection<Product> Products
        {
            get => _products;
            set => SetField(ref _products, value);
        }
    }
}
