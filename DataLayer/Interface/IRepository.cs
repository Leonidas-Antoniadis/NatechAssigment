namespace Natech.DataLayer.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
