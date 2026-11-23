using Marketplace.Application.Sellers;

namespace Marketplace.Api.Endpoints;

public static class SellerEndpoints
{
    public static IEndpointRouteBuilder MapSellerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/sellers").WithTags("Sellers");

        group.MapPost("/", async (CreateSellerRequest request, SellerService service, CancellationToken ct) =>
        {
            var seller = await service.CreateAsync(request, ct);
            return Results.Created($"/sellers/{seller.Id}", seller);
        });

        group.MapGet("/{id:guid}", async (Guid id, SellerService service, CancellationToken ct) =>
            Results.Ok(await service.GetAsync(id, ct)));

        return app;
    }
}
