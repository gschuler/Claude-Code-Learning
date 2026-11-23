using Marketplace.Application.Products;

namespace Marketplace.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapPost("/", async (CreateProductRequest request, ProductService service, CancellationToken ct) =>
        {
            var product = await service.CreateAsync(request, ct);
            return Results.Created($"/products/{product.Id}", product);
        });

        group.MapGet("/", async (ProductService service, CancellationToken ct) =>
            Results.Ok(await service.ListAsync(ct)));

        group.MapGet("/{id:guid}", async (Guid id, ProductService service, CancellationToken ct) =>
            Results.Ok(await service.GetAsync(id, ct)));

        return app;
    }
}
