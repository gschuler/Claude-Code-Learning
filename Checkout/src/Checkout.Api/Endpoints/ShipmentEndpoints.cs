using Checkout.Application.Shipments;

namespace Checkout.Api.Endpoints;

public static class ShipmentEndpoints
{
    public static IEndpointRouteBuilder MapShipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/shipments").WithTags("Shipments");

        group.MapGet("/{id:guid}", async (Guid id, ShipmentService service, CancellationToken ct) =>
            Results.Ok(await service.GetAsync(id, ct)));

        group.MapGet("/by-number/{number}", async (string number, ShipmentService service, CancellationToken ct) =>
            Results.Ok(await service.GetByNumberAsync(number, ct)));

        return app;
    }
}
