using Checkout.Domain.Carts;
using Checkout.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.Infrastructure.Persistence;

internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        // Cart items are part of the Cart aggregate: stored in their own table, loaded with the cart.
        builder.OwnsMany(c => c.Items, items =>
        {
            items.ToTable("CartItems");
            items.WithOwner().HasForeignKey("CartId");
            items.HasKey("CartId", nameof(CartItem.ProductId));
            items.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
            items.Property(i => i.UnitPrice).HasPrecision(18, 2);
            items.HasData(SeedData.CartItems);
        });
        builder.Navigation(c => c.Items).HasField("_items");

        builder.HasData(SeedData.Carts);
    }
}

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);

        builder.OwnsMany(o => o.Items, items =>
        {
            items.ToTable("OrderItems");
            items.WithOwner().HasForeignKey("OrderId");
            items.HasKey("OrderId", nameof(OrderItem.ProductId));
            items.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
            items.Property(i => i.UnitPrice).HasPrecision(18, 2);
            items.HasData(SeedData.OrderItems);
        });
        builder.Navigation(o => o.Items).HasField("_items");

        builder.HasData(SeedData.Orders);
    }
}
