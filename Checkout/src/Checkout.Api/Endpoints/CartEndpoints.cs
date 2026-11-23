using Checkout.Application.Carts;

namespace Checkout.Api.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/carts").WithTags("Carts");

        group.MapPost("/", async (CartService service, CancellationToken ct) =>
        {
            var cart = await service.CreateAsync(ct);
            return Results.Created($"/carts/{cart.Id}", cart);
        });

        group.MapGet("/{id:guid}", async (Guid id, CartService service, CancellationToken ct) =>
            Results.Ok(await service.GetAsync(id, ct)));

        group.MapPost("/{id:guid}/items", async (Guid id, AddCartItemRequest request, CartService service, CancellationToken ct) =>
            Results.Ok(await service.AddItemAsync(id, request, ct)));

        group.MapDelete("/{id:guid}/items/{productId:guid}", async (Guid id, Guid productId, CartService service, CancellationToken ct) =>
            Results.Ok(await service.RemoveItemAsync(id, productId, ct)));

        group.MapPost("/{id:guid}/checkout", async (Guid id, CartService service, CancellationToken ct) =>
        {
            var order = await service.CheckoutAsync(id, ct);
            return Results.Created($"/orders/{order.Id}", order);
        });

        return app;
    }
}
