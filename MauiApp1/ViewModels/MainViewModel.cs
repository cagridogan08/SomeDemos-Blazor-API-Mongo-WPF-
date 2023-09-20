
using DomainLibrary;
using Microsoft.AspNetCore.SignalR.Client;

namespace MauiApp1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly HubConnection _hubConnection;

        public MainViewModel(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            hubConnection.On<Product>("ProductCreated", ProductCreatedHandleReceived);
            hubConnection.StartAsync();
        }

        private void ProductCreatedHandleReceived(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
