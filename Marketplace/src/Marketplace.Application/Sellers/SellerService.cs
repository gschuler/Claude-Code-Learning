using Marketplace.Application.Abstractions;
using Marketplace.Application.Common;
using Marketplace.Domain.Sellers;

namespace Marketplace.Application.Sellers;

public sealed record CreateSellerRequest(string Name, string Email);

public sealed record SellerResponse(Guid Id, string Name, string Email)
{
    public static SellerResponse From(Seller s) => new(s.Id, s.Name, s.Email);
}

public sealed class SellerService(ISellerRepository sellers, IUnitOfWork unitOfWork)
{
    public async Task<SellerResponse> CreateAsync(CreateSellerRequest request, CancellationToken ct = default)
    {
        var seller = Seller.Create(request.Name, request.Email);
        await sellers.AddAsync(seller, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return SellerResponse.From(seller);
    }

    public async Task<SellerResponse> GetAsync(Guid id, CancellationToken ct = default)
    {
        var seller = await sellers.GetByIdAsync(id, ct) ?? throw new NotFoundException("Seller", id);
        return SellerResponse.From(seller);
    }
}
