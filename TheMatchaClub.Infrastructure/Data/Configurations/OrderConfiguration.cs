using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderId)
                .HasMaxLength(50);

            builder.Property(o => o.Timestamp)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(10,2)");



            builder.Property(o => o.Total)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.OrderType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Dine-In");

            builder.Property(o => o.CustomerName)
                .HasMaxLength(150);

            builder.Property(o => o.CustomerEmail)
                .HasMaxLength(255);

            // Navigation: Order has many OrderItems
            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            // Index for date-based reporting queries
            builder.HasIndex(o => o.Timestamp);
            builder.HasIndex(o => o.CustomerId);
        }
    }
}
