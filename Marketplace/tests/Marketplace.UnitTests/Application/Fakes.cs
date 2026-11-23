using Marketplace.Application.Abstractions;
using Marketplace.Domain.Products;
using Marketplace.Domain.Sellers;

namespace Marketplace.UnitTests.Application;

// Minimal hand-written fakes: keeps unit tests free of mocking libraries and of EF Core.

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}

internal sealed class FakeSellerRepository : ISellerRepository
{
    public List<Seller> Items { get; } = [];

    public Task AddAsync(Seller seller, CancellationToken ct = default)
    {
        Items.Add(seller);
        return Task.CompletedTask;
    }

    public Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(s => s.Id == id));

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.Any(s => s.Id == id));
}

internal sealed class FakeProductRepository : IProductRepository
{
    public List<Product> Items { get; } = [];

    public Task AddAsync(Product product, CancellationToken ct = default)
    {
        Items.Add(product);
        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(p => p.Id == id));

    public Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct = default) =>
        Task.FromResult<IReadOnlyList<Product>>(Items);
}
