using Checkout.Domain.Carts;
using Checkout.Domain.Orders;
using Checkout.Domain.Shipments;

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

public interface IShipmentRepository
{
    Task AddAsync(Shipment shipment, CancellationToken ct = default);
    Task<Shipment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Expects an already normalized number (see <see cref="Shipment.NormalizeNumber"/>).</summary>
    Task<Shipment?> GetByNumberAsync(string number, CancellationToken ct = default);
}
