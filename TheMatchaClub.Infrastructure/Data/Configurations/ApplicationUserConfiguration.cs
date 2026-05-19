using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Infrastructure.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for the extended ApplicationUser properties.
    /// Identity's built-in properties (UserName, Email, PasswordHash, etc.)
    /// are already configured by IdentityDbContext; this only handles
    /// the custom columns we added.
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
