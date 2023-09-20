using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MauiApp1.Platforms.Android;

namespace MauiApp1
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
                                                        | ConfigChanges.UiMode | ConfigChanges.ScreenLayout
                                                        | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {
            AndroidServiceManager.MainActivity = this;
        }

        public void StartService()
        {
            var serviceIntent = new Intent(this, typeof(MyBackgroundService));
            serviceIntent.PutExtra("inputExtra", "Background Service");
            StartService(serviceIntent);
        }

        public void StopService()
        {
            var serviceIntent = new Intent(this, typeof(MyBackgroundService));
            StopService(serviceIntent);
        }
    }

}