using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Infrastructure.Cache
{
    public static class CacheModule
    {
        public static void AddCacheModule(this IServiceCollection services, IConfiguration configuration)
        {
            #if DEBUG
            services.AddSingleton<IApplicationCache, DebugCache>();
            #endif
        }
    }
}