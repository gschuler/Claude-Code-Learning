using Checkout.Application.Abstractions;
using Checkout.Application.Common;
using Checkout.Domain.Shipments;

namespace Checkout.Application.Shipments;

public sealed record ShipmentResponse(Guid Id, string Number, Guid OrderId, string Carrier, DateTime CreatedAtUtc)
{
    public static ShipmentResponse From(Shipment s) => new(s.Id, s.Number, s.OrderId, s.Carrier, s.CreatedAtUtc);
}

public sealed class ShipmentService(IShipmentRepository shipments)
{
    public async Task<ShipmentResponse> GetAsync(Guid shipmentId, CancellationToken ct = default) =>
        ShipmentResponse.From(
            await shipments.GetByIdAsync(shipmentId, ct) ?? throw new NotFoundException("Shipment", shipmentId));

    /// <summary>Looks a shipment up by its number. Malformed numbers are a 400, unknown ones a 404.</summary>
    public async Task<ShipmentResponse> GetByNumberAsync(string number, CancellationToken ct = default)
    {
        var normalized = Shipment.NormalizeNumber(number);
        return ShipmentResponse.From(
            await shipments.GetByNumberAsync(normalized, ct) ?? throw new NotFoundException("Shipment", normalized));
    }
}
