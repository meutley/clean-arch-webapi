using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SourceName.Domain.Users;
using SourceName.Infrastructure.Postgres.Repositories;

namespace SourceName.Infrastructure.Postgres
{
    public static class PostgresModule
    {
        public static void AddPostgresModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString");
            services.AddDbContext<SourceNameContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}