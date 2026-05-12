using Microsoft.EntityFrameworkCore;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClub.Infrastructure.Data
{
    public class MatchaClubDbContext : DbContext
    {
        public MatchaClubDbContext(DbContextOptions<MatchaClubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<StoreSettings> StoreSettings => Set<StoreSettings>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatchaClubDbContext).Assembly);
        }
    }
}
