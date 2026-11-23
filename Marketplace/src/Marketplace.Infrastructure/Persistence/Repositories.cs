using Marketplace.Application.Abstractions;
using Marketplace.Domain.Products;
using Marketplace.Domain.Sellers;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

internal sealed class SellerRepository(MarketplaceDbContext db) : ISellerRepository
{
    public async Task AddAsync(Seller seller, CancellationToken ct = default) =>
        await db.Sellers.AddAsync(seller, ct);

    public Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Sellers.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        db.Sellers.AnyAsync(s => s.Id == id, ct);
}

internal sealed class ProductRepository(MarketplaceDbContext db) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken ct = default) =>
        await db.Products.AddAsync(product, ct);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct = default) =>
        await db.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);
}
