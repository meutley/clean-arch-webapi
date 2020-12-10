using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SourceName.Domain.Users;

namespace SourceName.Infrastructure.Postgres.Repositories
{
    public class UserRepository : RepositoryBase<string, User>, IUserRepository
    {
        public UserRepository(SourceNameContext context) : base(context) { }

        public Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return _context.Users
                .SingleOrDefaultAsync(
                    x => x.Email == email,
                    cancellationToken
                );
        }
    }
}