namespace MauiApp1.Platforms.Android
{
    public static class AndroidServiceManager
    {
        public static MainActivity? MainActivity { get; set; }

        public static bool IsRunning { get; set; }

        public static void StartService()
        {
            MainActivity?.StartService();
        }

        public static void StopService()
        {
            MainActivity?.StopService();
            IsRunning = false;
        }
    }
}
