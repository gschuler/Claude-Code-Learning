using Checkout.Domain.Carts;
using Checkout.Domain.Orders;
using Checkout.Domain.Shipments;
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

internal sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Number).HasMaxLength(20).IsRequired();
        builder.HasIndex(s => s.Number).IsUnique();
        builder.Property(s => s.Carrier).HasMaxLength(100).IsRequired();

        // References the Order aggregate by id only (no navigation), like Order → Cart.
        builder.HasOne<Order>().WithMany().HasForeignKey(s => s.OrderId);

        builder.HasData(SeedData.Shipments);
    }
}
