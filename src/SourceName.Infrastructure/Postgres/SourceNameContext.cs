using Microsoft.EntityFrameworkCore;

using SourceName.Domain.Users;

namespace SourceName.Infrastructure.Postgres
{
    public class SourceNameContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public SourceNameContext(DbContextOptions<SourceNameContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SourceNameContext).Assembly);
        }
    }
}