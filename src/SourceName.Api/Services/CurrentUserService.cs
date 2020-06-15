using System.Security.Claims;
using Microsoft.AspNetCore.Http;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId { get; }
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}