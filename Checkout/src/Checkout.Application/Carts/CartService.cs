using Checkout.Application.Abstractions;
using Checkout.Application.Common;
using Checkout.Application.Orders;
using Checkout.Domain.Carts;

namespace Checkout.Application.Carts;

public sealed record AddCartItemRequest(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity);

public sealed record CartItemResponse(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);

public sealed record CartResponse(Guid Id, bool IsCheckedOut, IReadOnlyList<CartItemResponse> Items, decimal Total)
{
    public static CartResponse From(Cart c) => new(
        c.Id,
        c.IsCheckedOut,
        c.Items.Select(i => new CartItemResponse(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.Subtotal)).ToList(),
        c.Total);
}

public sealed class CartService(ICartRepository carts, IOrderRepository orders, IUnitOfWork unitOfWork)
{
    public async Task<CartResponse> CreateAsync(CancellationToken ct = default)
    {
        var cart = Cart.Create();
        await carts.AddAsync(cart, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CartResponse.From(cart);
    }

    public async Task<CartResponse> GetAsync(Guid cartId, CancellationToken ct = default) =>
        CartResponse.From(await LoadAsync(cartId, ct));

    public async Task<CartResponse> AddItemAsync(Guid cartId, AddCartItemRequest request, CancellationToken ct = default)
    {
        var cart = await LoadAsync(cartId, ct);
        cart.AddItem(request.ProductId, request.ProductName, request.UnitPrice, request.Quantity);
        await unitOfWork.SaveChangesAsync(ct);
        return CartResponse.From(cart);
    }

    public async Task<CartResponse> RemoveItemAsync(Guid cartId, Guid productId, CancellationToken ct = default)
    {
        var cart = await LoadAsync(cartId, ct);
        cart.RemoveItem(productId);
        await unitOfWork.SaveChangesAsync(ct);
        return CartResponse.From(cart);
    }

    public async Task<OrderResponse> CheckoutAsync(Guid cartId, CancellationToken ct = default)
    {
        var cart = await LoadAsync(cartId, ct);
        var order = cart.Checkout();
        await orders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return OrderResponse.From(order);
    }

    private async Task<Cart> LoadAsync(Guid cartId, CancellationToken ct) =>
        await carts.GetByIdAsync(cartId, ct) ?? throw new NotFoundException("Cart", cartId);
}
