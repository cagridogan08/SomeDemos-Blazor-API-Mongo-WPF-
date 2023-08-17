using BlazorUIMongo.Collections;
using BlazorUIMongo.Data;

namespace BlazorUIMongo.Services
{
    public interface IService<T>
    {
        Task<ICollection<T>> Get();
        Task<bool> Add(T entity);
    }
    public class DriverService : IService<Driver>
    {
        private readonly HttpClient _httpClient;

        public DriverService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<Driver>> Get()
        {
            return await _httpClient.GetFromJsonAsync<Driver[]>("api/Driver");
        }

        public async Task<bool> Add(Driver entity)
        {
            var res = await _httpClient.PostAsJsonAsync<Driver>("api/Driver", entity);
            return res.IsSuccessStatusCode;
        }
    }
}
