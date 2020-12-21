using System.Threading;
using System.Threading.Tasks;

using Raven.Client.Documents;
using Raven.Client.Documents.Session;

using SourceName.Domain.Users;
using SourceName.Infrastructure.Raven.Indexes;

namespace SourceName.Infrastructure.Raven.Repositories
{
#if (UseRaven)
    public class UserRepository : RepositoryBase<string, User>, IUserRepository
    {
        public UserRepository(IAsyncDocumentSession session) : base(session) { }

        public Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return _session.Query<User, Users_ByEmail>()
                .SingleOrDefaultAsync(
                    x => x.Email == email,
                    cancellationToken
                );
        }
    }
#endif
}