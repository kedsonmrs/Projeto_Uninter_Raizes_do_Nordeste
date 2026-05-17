using System.Linq.Expressions;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IBaseRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        T Add(T entity);
        void Update (T entity);
        void Delete (T entity);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
