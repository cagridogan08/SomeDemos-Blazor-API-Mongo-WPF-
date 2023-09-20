using DomainLibrary;
using MauiApp1.Platforms.Android;
using MauiApp1.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
#if ANDROID
            MauiApp1.Platforms.Android.MyBackgroundService.NotificationReceived += MyBackgroundService_NotificationReceived;
            if (!MauiApp1.Platforms.Android.AndroidServiceManager.IsRunning)
            {
                MauiApp1.Platforms.Android.AndroidServiceManager.StartService();
                MessageLabel.Text = "Service has started";
            }
            else
            {
                MessageLabel.Text = "Service is running";
            }
#endif
        }

        private void MyBackgroundService_NotificationReceived(Product obj)
        {
            Label2.Text = obj.Name;
            UpdateChildrenLayout();
        }


        private void StopButton_Clicked(object sender, EventArgs e)
        {
#if ANDROID
            MauiApp1.Platforms.Android.AndroidServiceManager.StopService();
            MessageLabel.Text = "Service is stopped";
#endif
        }
    }
}