using Checkout.Application.Carts;
using Checkout.Application.Common;

namespace Checkout.UnitTests.Application;

public class CartServiceTests
{
    private readonly FakeCartRepository _carts = new();
    private readonly FakeOrderRepository _orders = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly CartService _service;

    public CartServiceTests() => _service = new CartService(_carts, _orders, _unitOfWork);

    [Fact]
    public async Task CheckoutAsync_stores_order_and_returns_total()
    {
        var cart = await _service.CreateAsync();
        await _service.AddItemAsync(cart.Id, new AddCartItemRequest(Guid.NewGuid(), "Keyboard", 100m, 2));

        var order = await _service.CheckoutAsync(cart.Id);

        Assert.Single(_orders.Items);
        Assert.Equal("Pending", order.Status);
        Assert.Equal(200m, order.Total);
    }

    [Fact]
    public async Task AddItemAsync_for_unknown_cart_throws_not_found()
    {
        var request = new AddCartItemRequest(Guid.NewGuid(), "Keyboard", 100m, 1);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.AddItemAsync(Guid.NewGuid(), request));
        Assert.Equal(0, _unitOfWork.SaveCount);
    }
}
