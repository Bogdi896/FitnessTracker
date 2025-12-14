using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly FitnessTrackerDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(FitnessTrackerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
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
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
