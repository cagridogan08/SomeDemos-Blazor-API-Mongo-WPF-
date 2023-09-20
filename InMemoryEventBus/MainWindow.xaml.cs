using System;
using System.Windows;
using InMemoryEventBus.ViewModels;

namespace InMemoryEventBus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }


        protected override async void OnContentRendered(EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                await vm.PublishAsync(75);
            }
            base.OnContentRendered(e);
        }
    }
}
