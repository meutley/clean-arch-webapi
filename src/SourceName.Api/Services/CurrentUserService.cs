using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Token { get; }
        public string UserId { get; }
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}