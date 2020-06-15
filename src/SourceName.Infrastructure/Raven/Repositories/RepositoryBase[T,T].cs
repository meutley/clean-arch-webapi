using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Raven.Client.Documents;
using Raven.Client.Documents.Session;

using SourceName.Domain.Common.Entities;
using SourceName.Domain.Common.Interfaces;

namespace SourceName.Infrastructure.Raven.Repositories
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
        where TKey : class
        where TEntity : BaseEntity<TKey>
    {
        protected readonly IAsyncDocumentSession _session;

        public RepositoryBase(IAsyncDocumentSession session) => _session = session;
        
        public virtual Task<TEntity> GetById(TKey id, string includePaths = "", CancellationToken cancellationToken = default)
        {
            var query = _session.Query<TEntity>();
            foreach (var includePath in includePaths?.Split(",").Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                query = query.Include(includePath);
            }
            return query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task Insert(TEntity entity, CancellationToken cancellationToken)
        {
            await _session.StoreAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }
        
        public virtual Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _session.SaveChangesAsync(cancellationToken);
        }
    }
}