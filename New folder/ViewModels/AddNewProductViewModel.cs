using System.Windows.Input;
using WpfAppWithRedisCache.Commands;
using WpfAppWithRedisCache.Models;

namespace WpfAppWithRedisCache.ViewModels
{
    public class AddNewProductViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private Product _newProduct;

        public AddNewProductViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _newProduct = new();
        }

        public Product NewProduct
        {
            get => _newProduct;
            set => SetField(ref _newProduct, value);
        }

        public ICommand AddCommand => new RelayCommand(() =>
        {
            _mainViewModel.Products.Add(NewProduct);
            NewProduct = new();
        });
    }
}
