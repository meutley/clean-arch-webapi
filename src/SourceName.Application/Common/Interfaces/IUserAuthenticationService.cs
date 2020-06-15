using SourceName.Domain.Users;

namespace SourceName.Application.Common.Interfaces
{
    public interface IUserAuthenticationService
    {
        string GenerateToken(User entity, string password);
    }
}