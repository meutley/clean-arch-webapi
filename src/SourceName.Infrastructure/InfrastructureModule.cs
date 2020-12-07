using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SourceName.Application.Common.Interfaces;
using SourceName.Infrastructure.Cache;
using SourceName.Infrastructure.Services;

namespace SourceName.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddScoped<IJwtBlacklistService, JwtBlacklistService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddCacheModule(configuration);
            services.AddPersistenceModule(configuration);
        }
    }
}