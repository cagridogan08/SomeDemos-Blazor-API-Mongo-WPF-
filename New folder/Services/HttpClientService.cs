using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iotech.Link.Libs.Modules.LinkRestAPI.DTOs;
using Iotech.Link.Libs.Modules.LinkRestAPI.Enums;
namespace WpfAppWithRedisCache.Services
{
    public interface IHttpClientService
    {
        HttpClient HttpClient { get; }
        Task<ICollection<T>> Get<T>();
        Task<bool> Create<T>(T t);
        Task<bool> Update<T>(IEnumerable<T> t);
        Task<bool> Delete<T>(IEnumerable<T> id);
        Task<bool> Create<T>(List<T> t);
    }

    public class HttpClientService : IHttpClientService
    {
        public HttpClient HttpClient { get; }
        public string NodeName { get; set; } = "249";

        public HttpClientService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public virtual async Task<ICollection<T>> Get<T>()
        {
            var items = await HttpClient.GetFromJsonAsync<T[]>($"api/{typeof(T).Name}");
            return items is not null ? items.ToList() : Enumerable.Empty<T>().ToList();
        }
        public virtual async Task<bool> Create<T>(T t)
        {
            var res = await HttpClient.PostAsJsonAsync($"api/{typeof(T).Name}?node={NodeName}", t);
            return res.IsSuccessStatusCode;
        }

        public virtual async Task<bool> Create<T>(List<T> t)
        {
            var response = await HttpClient.PostAsJsonAsync($"api/{typeof(T).Name}?node={NodeName}", t);
            var item = await response.Content.ReadFromJsonAsync<List<LinkApiResponseDto<T>>>();
            return item?.First().StatusCode == ApiStatusCode.Ok;
        }
        public virtual async Task<bool> Update<T>(IEnumerable<T> t)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/{typeof(T).Name}?node={NodeName}", t);
            var item = await response.Content.ReadFromJsonAsync<List<LinkApiResponseDto<T>>>();
            return item?.First().StatusCode == ApiStatusCode.Ok;
        }

        public virtual async Task<bool> Delete<T>(IEnumerable<T> id)
        {
            var response = await HttpClient.DeleteAsync($"api/{typeof(T).Name}?node={NodeName}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
