using System.Threading.Tasks;

namespace SourceName.Application.Common.Interfaces
{
    public interface IJwtBlacklistService
    {
        Task BlacklistToken(string token);
        Task<bool> IsTokenBlacklisted(string token);
    }
}