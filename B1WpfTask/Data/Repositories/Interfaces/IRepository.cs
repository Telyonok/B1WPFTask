using System.Linq.Expressions;

namespace B1WPFTask.Data.Repositories.Interfaces
{
    internal interface IRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll();
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
