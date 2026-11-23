using Checkout.Domain.Common;
using Checkout.Domain.Orders;

namespace Checkout.Domain.Carts;

public sealed class Cart
{
    private readonly List<CartItem> _items = [];

    public Guid Id { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public bool IsCheckedOut { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Subtotal);

    private Cart() { } // EF Core

    public static Cart Create() => new() { Id = Guid.NewGuid(), CreatedAtUtc = DateTime.UtcNow };

    /// <summary>Adds a product, or increases its quantity if it is already in the cart.</summary>
    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        EnsureOpen();
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var existing = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is null)
            _items.Add(new CartItem(productId, productName, unitPrice, quantity));
        else
            existing.IncreaseQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        EnsureOpen();
        var item = _items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new DomainException($"Product '{productId}' is not in the cart.");
        _items.Remove(item);
    }

    /// <summary>Closes the cart and turns its items into a pending order.</summary>
    public Order Checkout()
    {
        EnsureOpen();
        if (_items.Count == 0)
            throw new DomainException("Cannot check out an empty cart.");

        IsCheckedOut = true;
        return Order.Create(Id, _items.Select(i => new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity)));
    }

    private void EnsureOpen()
    {
        if (IsCheckedOut)
            throw new DomainException("Cart has already been checked out.");
    }
}
