using System.Linq;

using Raven.Client.Documents.Indexes;

using SourceName.Domain.Users;

namespace SourceName.Infrastructure.Raven.Indexes
{
    public class Users_ByEmail : AbstractIndexCreationTask<User>
    {
        public Users_ByEmail()
        {
            Map = users => users
                .Select(user => new { user.Email });
        }
    }
}