using System;

namespace WpfAppWithRedisCache.Services
{
    public interface ICacheService
    {
        T? GetData<T>(string key);
        bool SetData<T>(string key, T data, DateTimeOffset expireTime);

        object RemoveData(string key);
    }
}
