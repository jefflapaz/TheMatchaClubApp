using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Infrastructure.Data
{
    /// <summary>
    /// Main database context for the Matcha Club POS system.
    /// Inherits from IdentityDbContext to include ASP.NET Identity tables
    /// (AspNetUsers, AspNetRoles, AspNetUserRoles, etc.) alongside
    /// the application's business tables.
    /// </summary>
    public class MatchaClubDbContext : IdentityDbContext<ApplicationUser>
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
            // MUST call base — IdentityDbContext configures Identity table schemas here
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatchaClubDbContext).Assembly);
        }
    }
}
