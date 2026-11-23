using Marketplace.Application.Common;
using Marketplace.Application.Products;
using Marketplace.Domain.Sellers;

namespace Marketplace.UnitTests.Application;

public class ProductServiceTests
{
    private readonly FakeSellerRepository _sellers = new();
    private readonly FakeProductRepository _products = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ProductService _service;

    public ProductServiceTests() => _service = new ProductService(_products, _sellers, _unitOfWork);

    [Fact]
    public async Task CreateAsync_for_existing_seller_stores_product_and_saves()
    {
        var seller = Seller.Create("Acme", "sales@acme.test");
        _sellers.Items.Add(seller);

        var result = await _service.CreateAsync(new CreateProductRequest(seller.Id, "Mouse", 25m, "USD", 10));

        Assert.Single(_products.Items);
        Assert.Equal(1, _unitOfWork.SaveCount);
        Assert.Equal("Mouse", result.Name);
        Assert.Equal(25m, result.Price);
    }

    [Fact]
    public async Task CreateAsync_for_unknown_seller_throws_not_found()
    {
        var request = new CreateProductRequest(Guid.NewGuid(), "Mouse", 25m, "USD", 10);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
        Assert.Empty(_products.Items);
        Assert.Equal(0, _unitOfWork.SaveCount);
    }

    [Fact]
    public async Task GetAsync_for_unknown_product_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetAsync(Guid.NewGuid()));
    }
}
