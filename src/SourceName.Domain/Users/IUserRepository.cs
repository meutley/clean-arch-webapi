using System.Threading;
using System.Threading.Tasks;

using SourceName.Domain.Common.Interfaces;

namespace SourceName.Domain.Users
{
    public interface IUserRepository : IRepository<string, User>
    {
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
    }
}