using BlazorUIMongo.Collections;
using BlazorUIMongo.Data;
using BlazorUIMongo.Services;

namespace BlazorUIMongo.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(IService<Driver> service)
        {
            _drivers = new ServiceSyncCollection<Driver>(service);
        }

        private ServiceSyncCollection<Driver> _drivers;

        public ServiceSyncCollection<Driver> Drivers
        {
            get => _drivers;
            set => SetField(ref _drivers, value);
        }
    }
}
