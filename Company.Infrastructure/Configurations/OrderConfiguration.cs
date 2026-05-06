using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Company.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.BuyerEmail)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔥 Address (Owned Type)
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();

                a.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
                a.Property(a => a.LastName).IsRequired().HasMaxLength(50);
                a.Property(a => a.Street).IsRequired().HasMaxLength(100);
                a.Property(a => a.City).IsRequired().HasMaxLength(50);
                a.Property(a => a.Country).IsRequired().HasMaxLength(50);
            });

            // 🔥 Relationship: Order → OrderItems
            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
