using Checkout.Domain.Carts;
using Checkout.Domain.Orders;

namespace Checkout.Application.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface ICartRepository
{
    Task AddAsync(Cart cart, CancellationToken ct = default);
    Task<Cart?> GetByIdAsync(Guid id, CancellationToken ct = default);
}

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
