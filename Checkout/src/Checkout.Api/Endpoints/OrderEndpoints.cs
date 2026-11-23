using Checkout.Application.Orders;

namespace Checkout.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapGet("/{id:guid}", async (Guid id, OrderService service, CancellationToken ct) =>
            Results.Ok(await service.GetAsync(id, ct)));

        group.MapPost("/{id:guid}/pay", async (Guid id, OrderService service, CancellationToken ct) =>
            Results.Ok(await service.PayAsync(id, ct)));

        group.MapPost("/{id:guid}/cancel", async (Guid id, OrderService service, CancellationToken ct) =>
            Results.Ok(await service.CancelAsync(id, ct)));

        return app;
    }
}
