using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#if (UseRaven)
using SourceName.Infrastructure.Raven;
#elif (UsePg)
using SourceName.Infrastructure.Postgres;
#endif

namespace SourceName.Infrastructure
{
    internal static class PersistenceModule
    {
        internal static void AddPersistenceModule(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
#if (UseRaven)
            services.AddRavenModule();
#elif (UsePg)
            services.AddPostgresModule(configuration);
#endif
        }
    }
}