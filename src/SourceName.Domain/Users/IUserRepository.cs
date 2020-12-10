using System.Threading;
using System.Threading.Tasks;

using SourceName.Domain.Common.Interfaces;

namespace SourceName.Domain.Users
{
    #if (UseRaven)
    public interface IUserRepository : IRepository<string, User>
    {
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
    }
    #else
    public interface IUserRepository : IRepository<int, User>
    {
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
    }
    #endif
}