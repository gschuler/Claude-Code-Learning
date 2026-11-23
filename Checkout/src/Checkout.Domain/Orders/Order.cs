using Checkout.Domain.Common;

namespace Checkout.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Paid,
    Cancelled
}

public sealed record OrderItem(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }
    public Guid CartId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Subtotal);

    private Order() { } // EF Core

    internal static Order Create(Guid cartId, IEnumerable<OrderItem> items)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CartId = cartId,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
        order._items.AddRange(items);
        return order;
    }

    public void Pay()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Only pending orders can be paid (current status: {Status}).");
        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Only pending orders can be cancelled (current status: {Status}).");
        Status = OrderStatus.Cancelled;
    }
}
