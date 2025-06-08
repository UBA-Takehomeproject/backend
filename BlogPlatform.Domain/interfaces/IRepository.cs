using System.Linq.Expressions;

namespace BlogPlatform.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id,
         params Expression<Func<T, object>>[] includes
        );
        Task<IEnumerable<T>> GetAllAsync(int from, int count);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                 params Expression<Func<T, object>>[] includes
        );
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetAsync(
             Expression<Func<T, bool>> predicate,
             params Expression<Func<T, object>>[] includes
        );
        // Task<T?> GetAllAsync(
        //      Expression<Func<T, bool>> predicate,
        //      params Expression<Func<T, object>>[] includes
        // );




        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includes
        );
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
