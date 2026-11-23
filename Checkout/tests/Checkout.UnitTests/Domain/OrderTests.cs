using Checkout.Domain.Carts;
using Checkout.Domain.Common;
using Checkout.Domain.Orders;

namespace Checkout.UnitTests.Domain;

public class OrderTests
{
    private static Order NewPendingOrder()
    {
        var cart = Cart.Create();
        cart.AddItem(Guid.NewGuid(), "Keyboard", 100m, 1);
        return cart.Checkout();
    }

    [Fact]
    public void Pay_pending_order_marks_it_paid()
    {
        var order = NewPendingOrder();

        order.Pay();

        Assert.Equal(OrderStatus.Paid, order.Status);
    }

    [Fact]
    public void Pay_cancelled_order_throws()
    {
        var order = NewPendingOrder();
        order.Cancel();

        Assert.Throws<DomainException>(order.Pay);
    }

    [Fact]
    public void Cancel_paid_order_throws()
    {
        var order = NewPendingOrder();
        order.Pay();

        Assert.Throws<DomainException>(order.Cancel);
    }
}
