using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace WpfAppWithRedisCache.Extensions
{
    internal static class TaskExtensions
    {
        public static async void Await(this Task task, Action? onComplete = null, Action<System.Exception>? onError = null)
        {
            try
            {
                await task;
                onComplete?.Invoke();
            }
            catch (System.Exception e)
            {
                onError?.Invoke(e);
            }
        }
        public static async void Await<T>(this Task<T> task, Action<T>? onComplete = null, Action<Exception>? onError = null)
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
