using Android.App;
using Android.Content;
using Android.Widget;
using AndroidX.Core.Content;

namespace MauiApp1.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent is not null && intent.Action == Intent.ActionAirplaneModeChanged)
            {
                Toast.MakeText(context, "Boot completed event received", ToastLength.Short)?.Show();

                var serviceIntent = new Intent(context, typeof(MyBackgroundService));

                ContextCompat.StartForegroundService(context, serviceIntent);
            }
        }
    }
}
