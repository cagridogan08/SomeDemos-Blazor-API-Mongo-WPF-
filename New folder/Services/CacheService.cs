using System;
using System.Text.Json;
using StackExchange.Redis;

namespace WpfAppWithRedisCache.Services
{
    internal class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService()
        {
            _database = ConnectionMultiplexer.Connect(App.GetConfiguration("RedisURL")!).GetDatabase();
        }

        public T? GetData<T>(string key)
        {
            var data = _database.StringGet(key);
            return !string.IsNullOrEmpty(data) ? JsonSerializer.Deserialize<T>(data!) : default;
        }

        public bool SetData<T>(string key, T data, DateTimeOffset expireTime)
        {
            var expirationTime = expireTime.DateTime.Subtract(DateTime.Now);
            var isSet = _database.StringSet(key, JsonSerializer.Serialize(data), expirationTime);
            return isSet;
        }

        public object RemoveData(string key)
        {
            var isExist = _database.KeyExists(key);
            return isExist && _database.KeyDelete(key);
        }
    }
}
