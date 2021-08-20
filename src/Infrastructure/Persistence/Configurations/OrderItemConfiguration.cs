﻿using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(u => u.ProductPrice)
                .HasPrecision(16, 4)
                .IsRequired();

            builder.Property(u => u.Quantity)
                .IsRequired();

            builder.Property(u => u.Discount)
                .IsRequired();

            builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
