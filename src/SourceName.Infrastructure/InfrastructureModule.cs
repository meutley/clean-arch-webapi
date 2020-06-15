using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SourceName.Application.Common.Interfaces;
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
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddPersistenceModule(configuration);
        }
    }
}