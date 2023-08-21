using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WpfAppWithRedisCache.Services
{
    public interface IHttpClientService<T> where T : class
    {
        Task<ICollection<T>> Get();
        Task<bool> Create(T t);
        Task<bool> Update(T t);
        Task<bool> Delete(T id);
    }
    internal class HttpClientService<T> : IHttpClientService<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<T>> Get()
        {
            var items = await _httpClient.GetFromJsonAsync<T[]>($"api/{typeof(T).Name}");
            return items is not null ? items.ToList() : Enumerable.Empty<T>().ToList();
        }

        public async Task<bool> Create(T t)
        {
            var res = await _httpClient.PostAsJsonAsync<T>($"api/{typeof(T).Name}", t);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> Update(T t)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/{typeof(T).Name}", t);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(T id)
        {
            var response = await _httpClient.DeleteAsync($"api/{typeof(T).Name}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
