using System.Windows.Input;
using WpfAppWithRedisCache.Commands;
using WpfAppWithRedisCache.Models;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public MainViewModel(IDataService<Product> service)
        {
            _products = new ServiceSyncCollection<Product>(service);
        }
        private ServiceSyncCollection<Product> _products;

        public ServiceSyncCollection<Product> Products
        {
            get => _products;
            set => SetField(ref _products, value);
        }

        public ICommand DeleteItemCommand => new RelayCommand<Product>((product =>
        {
            Products.Remove(product);
        }));
    }
}
