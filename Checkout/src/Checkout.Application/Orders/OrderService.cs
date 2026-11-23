using Checkout.Application.Abstractions;
using Checkout.Application.Common;
using Checkout.Domain.Orders;

namespace Checkout.Application.Orders;

public sealed record OrderItemResponse(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);

public sealed record OrderResponse(Guid Id, Guid CartId, string Status, IReadOnlyList<OrderItemResponse> Items, decimal Total)
{
    public static OrderResponse From(Order o) => new(
        o.Id,
        o.CartId,
        o.Status.ToString(),
        o.Items.Select(i => new OrderItemResponse(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.Subtotal)).ToList(),
        o.Total);
}

public sealed class OrderService(IOrderRepository orders, IUnitOfWork unitOfWork)
{
    public async Task<OrderResponse> GetAsync(Guid orderId, CancellationToken ct = default) =>
        OrderResponse.From(await LoadAsync(orderId, ct));

    public async Task<OrderResponse> PayAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await LoadAsync(orderId, ct);
        order.Pay();
        await unitOfWork.SaveChangesAsync(ct);
        return OrderResponse.From(order);
    }

    public async Task<OrderResponse> CancelAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await LoadAsync(orderId, ct);
        order.Cancel();
        await unitOfWork.SaveChangesAsync(ct);
        return OrderResponse.From(order);
    }

    private async Task<Order> LoadAsync(Guid orderId, CancellationToken ct) =>
        await orders.GetByIdAsync(orderId, ct) ?? throw new NotFoundException("Order", orderId);
}
