using System.Windows;
using System.Windows.Controls;
using WpfAppWithRedisCache.ViewModels;

namespace WpfAppWithRedisCache.Views
{
    /// <summary>
    /// Interaction logic for AddNewProductView.xaml
    /// </summary>
    public partial class AddNewProductView
    {
        public AddNewProductView()
        {
            InitializeComponent();
            var vm = Application.Current as App;
            DataContext = new AddNewProductViewModel(vm?.GetRequiredService<MainViewModel>());
        }
    }
}
