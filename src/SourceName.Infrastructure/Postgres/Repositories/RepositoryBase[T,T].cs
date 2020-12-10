using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SourceName.Domain.Common.Entities;
using SourceName.Domain.Common.Interfaces;

namespace SourceName.Infrastructure.Postgres.Repositories
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
        where TKey : class
        where TEntity : BaseEntity<TKey>
    {
        protected readonly SourceNameContext _context;

        public RepositoryBase(SourceNameContext context) => _context = context;
        
        public virtual Task<TEntity> GetById(TKey id, string includePaths = "", CancellationToken cancellationToken = default)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            foreach (var includePath in includePaths?.Split(",").Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                query = query.Include(includePath);
            }
            return query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task Insert(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Set<TEntity>().Add(entity);
            await SaveChangesAsync(cancellationToken);
        }
        
        public virtual Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}