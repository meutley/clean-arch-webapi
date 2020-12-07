using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SourceName.Application.Common.Configuration;
using SourceName.Application.Common.Interfaces;

namespace SourceName.Infrastructure.Services
{
    public class JwtBlacklistService : IJwtBlacklistService
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IApplicationCache _cache;

        public JwtBlacklistService(
            IOptionsSnapshot<AuthenticationConfiguration> authenticationConfiguration,
            IApplicationCache cache
        )
        {
            _authenticationConfiguration = authenticationConfiguration.Value;
            _cache = cache;
        }
        
        public Task BlacklistToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(token);
            }
            
            var cacheKey = $"JWT_BLACKLIST_${token}";
            return _cache.AddOrUpdate(
                cacheKey,
                token,
                TimeSpan.FromSeconds(_authenticationConfiguration.TokenLifetimeInSeconds)
            );
        }

        public async Task<bool> IsTokenBlacklisted(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            
            var cacheKey = $"JWT_BLACKLIST_${token}";
            var cachedValue = await _cache.Get(cacheKey);
            return cachedValue is object;
        }
    }
}