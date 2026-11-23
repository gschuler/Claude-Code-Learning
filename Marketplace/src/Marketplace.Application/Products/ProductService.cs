using Marketplace.Application.Abstractions;
using Marketplace.Application.Common;
using Marketplace.Domain.Common;
using Marketplace.Domain.Products;

namespace Marketplace.Application.Products;

public sealed record CreateProductRequest(Guid SellerId, string Name, decimal Price, string Currency, int Stock);

public sealed record ProductResponse(Guid Id, Guid SellerId, string Name, decimal Price, string Currency, int Stock)
{
    public static ProductResponse From(Product p) =>
        new(p.Id, p.SellerId, p.Name, p.Price.Amount, p.Price.Currency, p.Stock);
}

public sealed class ProductService(IProductRepository products, ISellerRepository sellers, IUnitOfWork unitOfWork)
{
    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        if (!await sellers.ExistsAsync(request.SellerId, ct))
            throw new NotFoundException("Seller", request.SellerId);

        var price = new Money(request.Price, request.Currency);
        var product = Product.Create(request.SellerId, request.Name, price, request.Stock);
        await products.AddAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return ProductResponse.From(product);
    }

    public async Task<ProductResponse> GetAsync(Guid id, CancellationToken ct = default)
    {
        var product = await products.GetByIdAsync(id, ct) ?? throw new NotFoundException("Product", id);
        return ProductResponse.From(product);
    }

    public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await products.ListAsync(ct);
        return list.Select(ProductResponse.From).ToList();
    }
}
