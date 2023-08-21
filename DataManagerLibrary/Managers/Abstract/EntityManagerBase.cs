

using DataManagerLibrary.Context;

namespace DataManagerLibrary.Managers.Abstract
{
    public abstract class EntityManagerBase<T> : IEntityManager<T> where T : class
    {
        protected readonly ApplicationDataContext _context;

        protected EntityManagerBase(ApplicationDataContext context)
        {
            _context = context;
        }

        public abstract Task<ICollection<T>> GetAllAsync();

        public abstract Task<T?> GetAsync(int id);

        public abstract Task<bool> AddAsync(T entity);

        public abstract Task<bool> UpdateAsync(T entity);

        public abstract Task<bool> DeleteAsync(int id);


    }
}
