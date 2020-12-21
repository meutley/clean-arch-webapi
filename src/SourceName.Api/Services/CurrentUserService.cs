using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Token { get; }
#if (UseRaven)
        public string UserId { get; }
#else
        public int? UserId { get; }
#endif
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(userId))
            {
#if (UseRaven)
                UserId = userId;
#else
                UserId = int.Parse(userId);
#endif
            }
        }
    }
}