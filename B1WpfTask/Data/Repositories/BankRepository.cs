using B1WPFTask.Data.Repositories.Interfaces;
using B1WPFTask.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1WPFTask.Data.Repositories
{
    internal class BankRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly BankDBContext _context;

        public BankRepository(BankDBContext context) => _context = context;

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression);

        public IQueryable<T> GetAll() => _context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _context.Set<T>().Where(entity => entity.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
