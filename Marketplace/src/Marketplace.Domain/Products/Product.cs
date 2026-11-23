using Marketplace.Domain.Common;

namespace Marketplace.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public Guid SellerId { get; private set; }
    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public int Stock { get; private set; }

    private Product() { } // EF Core

    public static Product Create(Guid sellerId, string name, Money price, int stock)
    {
        if (sellerId == Guid.Empty)
            throw new DomainException("Seller is required.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");
        if (price.Amount <= 0)
            throw new DomainException("Price must be greater than zero.");
        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        return new Product { Id = Guid.NewGuid(), SellerId = sellerId, Name = name.Trim(), Price = price, Stock = stock };
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
        if (quantity > Stock)
            throw new DomainException($"Insufficient stock: requested {quantity}, available {Stock}.");

        Stock -= quantity;
    }
}
