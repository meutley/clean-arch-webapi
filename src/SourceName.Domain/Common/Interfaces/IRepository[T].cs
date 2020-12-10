using System.Threading;
using System.Threading.Tasks;

using SourceName.Domain.Common.Entities;

namespace SourceName.Domain.Common.Interfaces
{
    public interface IRepository<TKey, TEntity>
        where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetById(TKey id, string includePaths = "", CancellationToken cancellationToken = default);
        Task Insert(TEntity entity, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}