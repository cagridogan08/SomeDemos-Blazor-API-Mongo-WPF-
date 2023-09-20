using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using DomainLibrary;
using Microsoft.AspNetCore.SignalR.Client;

namespace MauiApp1.Platforms.Android
{
    [Service]
    internal class MyBackgroundService : Service
    {
        Timer timer = null;
        int myId = (new object()).GetHashCode();
        int BadgeNumber = 0;
        private NotificationCompat.Builder notification;
        private HubConnection hubConnection;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent,
            StartCommandFlags flags, int startId)
        {
            var input = intent.GetStringExtra("inputExtra");

            var notificationIntent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
                PendingIntentFlags.Immutable);

            var notification = new NotificationCompat.Builder(this,
                    MainApplication.ChannelId)
                .SetContentText(input)
                .SetSmallIcon(Resource.Drawable.ic_clock_black_24dp)
                .SetContentIntent(pendingIntent);

            // Increment the BadgeNumber
            BadgeNumber++;
            // set the number
            notification.SetNumber(BadgeNumber);
            // set the title (text) to Service Running
            notification.SetContentTitle("Service Running");
            // build and notify
            StartForeground(myId, notification.Build());

            // timer to ensure hub connection
            timer = new Timer(Timer_Elapsed, notification, 0, 10000);

            // You can stop the service from inside the service by calling StopSelf();

            return StartCommandResult.Sticky;
        }

        public static Action<Product> NotificationReceived;
        async Task EnsureHubConnection()
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://192.168.3.69:5041/messages")
                    .Build();

                hubConnection.On<Product>("ProductCreated", (message) =>
                {
                    NotificationReceived.Invoke(message);
                    // Display the message in a notification
                    BadgeNumber++;
                    notification.SetNumber(BadgeNumber);
                    notification.SetContentTitle(message.Name);
                    StartForeground(myId, notification.Build());
                });
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception e)
                {
                    // Put a breakpoint on the next line to debug
                }

            }
            else if (hubConnection.State != HubConnectionState.Connected)
            {
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception e)
                {
                    // Put a breakpoint on the next line to debug
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        async void Timer_Elapsed(object state)
        {
            AndroidServiceManager.IsRunning = true;

            await EnsureHubConnection();

        }
    }
}
