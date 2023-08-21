using System.Windows;
using System.Windows.Input;
using DomainLibrary;
using Microsoft.AspNetCore.SignalR.Client;
using WpfAppWithRedisCache.Commands;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public MainViewModel(IDataService<Product> service, HubConnection hubConnection)
        {
            _products = new ServiceSyncCollection<Product>(service);
            hubConnection.On<Product>("ProductCreated", ProductCreatedHandleReceived);
            hubConnection.StartAsync();
        }

        private void ProductCreatedHandleReceived(Product obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Products.Add(obj);
            });
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
