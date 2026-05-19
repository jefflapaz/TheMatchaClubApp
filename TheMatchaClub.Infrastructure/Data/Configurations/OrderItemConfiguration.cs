using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            // Composite key not ideal here; add a surrogate key via shadow property
            builder.Property<int>("Id")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");

            // Shadow property for the FK back to Order
            builder.Property<string>("OrderId")
                .HasMaxLength(50);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.CategoryName)
                .HasMaxLength(100);

            builder.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");

            // LineTotal is computed in-memory; ignore it for database mapping
            builder.Ignore(oi => oi.LineTotal);
        }
    }
}
