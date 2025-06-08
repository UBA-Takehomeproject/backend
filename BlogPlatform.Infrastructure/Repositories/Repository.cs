using System.Linq.Expressions;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(string id) => await _context.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Remove(T entity) => _context.Set<T>().Remove(entity);
        public async Task<IEnumerable<T>> GetAllAsync(int from, int count) => await _context.Set<T>().Skip(from).Take(count).ToListAsync();
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.CountAsync(predicate);
        }

        public Task<int> CountAsync()
        {
            return _dbSet.CountAsync();
        }
    }
}
