namespace BlazorUIMongo.Services
{
    public abstract class ServiceBase<T> : IService<T>
    {
        private readonly HttpClient _httpClient;

        protected ServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<T>> Get()
        {
            var items = await _httpClient.GetFromJsonAsync<T[]>($"api/{typeof(T).Name}");
            return items is not null ? items.ToList() : Enumerable.Empty<T>().ToList();
        }

        public async Task<bool> Add(T entity)
        {
            var res = await _httpClient.PostAsJsonAsync<T>($"api/{typeof(T).Name}", entity);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> Update(T entity)
        {
            var response = await _httpClient.PutAsJsonAsync<T>($"api/{typeof(T).Name}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(T id)
        {
            var response = await _httpClient.DeleteAsync($"api/{typeof(T).Name}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
