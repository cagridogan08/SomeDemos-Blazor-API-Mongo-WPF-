using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLibrary;

namespace WpfAppWithRedisCache.Services
{
    public interface IDataService<T> where T : Entity
    {

        Task<ICollection<T>> GetData();

        Task<T?> GetDataOrNull(int id);

        Task<bool> RemoveData(int id);

        Task<bool> AddData(T data);

        Task<bool> UpdateData(T data);
    }
}
