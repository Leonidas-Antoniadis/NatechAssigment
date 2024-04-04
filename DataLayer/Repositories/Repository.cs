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

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}