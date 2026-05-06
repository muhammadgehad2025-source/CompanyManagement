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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            // 🔥 Product snapshot (Owned Type)
            builder.OwnsOne(i => i.ItemOrdered, io =>
            {
                io.WithOwner();

                io.Property(p => p.ProductName)
                  .IsRequired()
                  .HasMaxLength(100);

                io.Property(p => p.PictureUrl)
                  .IsRequired();
            });
        }
    }
}
