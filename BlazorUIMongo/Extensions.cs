namespace BlazorUIMongo
{
    public static class Extensions
    {
        public static async void Await<T>(this Task<T> task, Action<T>? onComplete = null, Action<System.Exception>? onError = null)
        {
            try
            {
                onComplete?.Invoke(await task);
            }
            catch (System.Exception e)
            {
                onError?.Invoke(e);
            }
        }
    }
}
