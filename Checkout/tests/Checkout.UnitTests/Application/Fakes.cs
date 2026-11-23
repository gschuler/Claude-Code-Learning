using Checkout.Application.Abstractions;
using Checkout.Domain.Carts;
using Checkout.Domain.Orders;

namespace Checkout.UnitTests.Application;

// Minimal hand-written fakes: keeps unit tests free of mocking libraries and of EF Core.

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}

internal sealed class FakeCartRepository : ICartRepository
{
    public List<Cart> Items { get; } = [];

    public Task AddAsync(Cart cart, CancellationToken ct = default)
    {
        Items.Add(cart);
        return Task.CompletedTask;
    }

    public Task<Cart?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(c => c.Id == id));
}

internal sealed class FakeOrderRepository : IOrderRepository
{
    public List<Order> Items { get; } = [];

    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        Items.Add(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(o => o.Id == id));
}
