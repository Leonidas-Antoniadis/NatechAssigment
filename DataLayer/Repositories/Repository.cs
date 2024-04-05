using DataLayer;
using Microsoft.EntityFrameworkCore;
using Natech.DataLayer.Interface;

namespace Natech.DataLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly NatechAssignmentContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(NatechAssignmentContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> FindAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public void UpdateAsync(T entity) =>
            _dbSet.Update(entity);

        public Task SaveChangesAsync() =>
            _dbContext.SaveChangesAsync();

        public void DeleteAsync(T entity) =>
            _dbSet.Remove(entity);

    }
}