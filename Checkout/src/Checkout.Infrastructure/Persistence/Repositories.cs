using Checkout.Application.Abstractions;
using Checkout.Domain.Carts;
using Checkout.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence;

internal sealed class CartRepository(CheckoutDbContext db) : ICartRepository
{
    public async Task AddAsync(Cart cart, CancellationToken ct = default) =>
        await db.Carts.AddAsync(cart, ct);

    public Task<Cart?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Carts.FirstOrDefaultAsync(c => c.Id == id, ct);
}

internal sealed class OrderRepository(CheckoutDbContext db) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken ct = default) =>
        await db.Orders.AddAsync(order, ct);

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Orders.FirstOrDefaultAsync(o => o.Id == id, ct);
}
