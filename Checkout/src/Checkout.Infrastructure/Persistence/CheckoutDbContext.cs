using Checkout.Application.Abstractions;
using Checkout.Domain.Carts;
using Checkout.Domain.Orders;
using Checkout.Domain.Shipments;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence;

public sealed class CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CheckoutDbContext).Assembly);

    async Task IUnitOfWork.SaveChangesAsync(CancellationToken ct) => await SaveChangesAsync(ct);
}
