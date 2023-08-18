using System.Collections.Generic;
using DomainLibrary;

namespace WpfAppWithRedisCache.Services
{
    public interface IDataService<T> where T : Entity
    {
        IDataService<T> GetDataService();

        ICollection<T> GetData();

        T? GetDataOrNull(int id);

        bool RemoveData(int id);

        bool AddData(T data);

        bool UpdateData(T data);
    }
}
