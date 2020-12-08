using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SourceName.Infrastructure.Raven;

namespace SourceName.Infrastructure
{
    internal static class PersistenceModule
    {
        private const string RAVEN = "RAVEN";
        
        internal static void AddPersistenceModule(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var databaseProvider = configuration.GetValue("Database:Provider", string.Empty);
            if (string.IsNullOrWhiteSpace(databaseProvider))
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }

            switch (databaseProvider.ToUpper())
            {
                case RAVEN:
                    services.AddRavenModule();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider));
            }
        }
    }
}