using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClub.Infrastructure.Data.Configurations
{
    public class StoreSettingsConfiguration : IEntityTypeConfiguration<StoreSettings>
    {
        public void Configure(EntityTypeBuilder<StoreSettings> builder)
        {
            builder.ToTable("StoreSettings");

            // StoreSettings is a single-row table; use a shadow Id property
            builder.Property<int>("Id")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");

            builder.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.StoreLogoPath)
                .HasMaxLength(500);

            builder.Property(s => s.Email)
                .HasMaxLength(255);

            builder.Property(s => s.Phone)
                .HasMaxLength(20);

            builder.Property(s => s.Address)
                .HasMaxLength(500);

            // Seed default store settings
            builder.HasData(new
            {
                Id = 1,
                StoreName = "The Matcha Club",
                StoreLogoPath = "",
                Email = "info@thematchaclub.ph",
                Phone = "+63 912 345 6789",
                Address = "Makati City, Metro Manila"
            });
        }
    }
}
