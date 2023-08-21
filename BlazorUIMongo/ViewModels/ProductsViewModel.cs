using BlazorUIMongo.Collections;
using BlazorUIMongo.Services;
using DomainLibrary;

namespace BlazorUIMongo.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        public ProductsViewModel(IService<Product> service)
        {
            _products = new ServiceSyncCollection<Product>(service);
        }
        private ServiceSyncCollection<Product> _products;

        public ServiceSyncCollection<Product> Products
        {
            get => _products;
            set => SetField(ref _products, value);
        }
    }
}
