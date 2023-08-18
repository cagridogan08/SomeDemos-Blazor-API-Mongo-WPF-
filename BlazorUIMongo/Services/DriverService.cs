using BlazorUIMongo.Data;

namespace BlazorUIMongo.Services
{
    public interface IService<T>
    {
        Task<ICollection<T>> Get();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T id);
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

        public async Task<bool> Update(Driver entity)
        {
            var res = await _httpClient.PutAsJsonAsync<Driver>("api/Driver", entity);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(Driver id)
        {
            var res = await _httpClient.DeleteAsync($"api/Driver/{id.Id}");
            return res.IsSuccessStatusCode;
        }
    }
}
