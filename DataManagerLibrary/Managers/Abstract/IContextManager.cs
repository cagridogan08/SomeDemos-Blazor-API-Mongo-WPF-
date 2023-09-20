namespace DataManagerLibrary.Managers.Abstract
{
    public interface IEntityManager<T>
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<T?> GetAsync(int id);

        public Task<bool> AddAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(int id);

    }
}
