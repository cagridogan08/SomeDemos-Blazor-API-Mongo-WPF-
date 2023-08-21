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
    public class DriverService : ServiceBase<Driver>
    {
        public DriverService(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
