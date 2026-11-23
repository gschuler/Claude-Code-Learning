using Checkout.Domain.Carts;
using Checkout.Domain.Common;
using Checkout.Domain.Orders;

namespace Checkout.UnitTests.Domain;

public class CartTests
{
    private static readonly Guid Keyboard = Guid.NewGuid();
    private static readonly Guid Mouse = Guid.NewGuid();

    [Fact]
    public void AddItem_same_product_twice_merges_quantity()
    {
        var cart = Cart.Create();

        cart.AddItem(Keyboard, "Keyboard", 100m, 1);
        cart.AddItem(Keyboard, "Keyboard", 100m, 2);

        var item = Assert.Single(cart.Items);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(300m, cart.Total);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddItem_with_non_positive_quantity_throws(int quantity)
    {
        var cart = Cart.Create();

        Assert.Throws<DomainException>(() => cart.AddItem(Keyboard, "Keyboard", 100m, quantity));
    }

    [Fact]
    public void RemoveItem_removes_product_from_cart()
    {
        var cart = Cart.Create();
        cart.AddItem(Keyboard, "Keyboard", 100m, 1);
        cart.AddItem(Mouse, "Mouse", 20m, 1);

        cart.RemoveItem(Keyboard);

        Assert.Equal(Mouse, Assert.Single(cart.Items).ProductId);
    }

    [Fact]
    public void Checkout_empty_cart_throws()
    {
        Assert.Throws<DomainException>(() => Cart.Create().Checkout());
    }

    [Fact]
    public void Checkout_creates_pending_order_and_closes_cart()
    {
        var cart = Cart.Create();
        cart.AddItem(Keyboard, "Keyboard", 100m, 1);
        cart.AddItem(Mouse, "Mouse", 20m, 2);

        var order = cart.Checkout();

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(cart.Id, order.CartId);
        Assert.Equal(140m, order.Total);
        Assert.True(cart.IsCheckedOut);
        Assert.Throws<DomainException>(() => cart.AddItem(Mouse, "Mouse", 20m, 1));
    }
}
