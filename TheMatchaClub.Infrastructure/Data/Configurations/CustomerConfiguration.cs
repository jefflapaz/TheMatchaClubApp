using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClub.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Email)
                .HasMaxLength(255);

            builder.Property(c => c.Phone)
                .HasMaxLength(20);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("New");

            builder.Property(c => c.AdminNotes)
                .HasMaxLength(1000);

            builder.Property(c => c.ProfileImagePath)
                .HasMaxLength(500);

            builder.Property(c => c.MemberSince)
                .HasDefaultValueSql("GETDATE()");

            // Index for quick email lookups
            builder.HasIndex(c => c.Email);
        }
    }
}
