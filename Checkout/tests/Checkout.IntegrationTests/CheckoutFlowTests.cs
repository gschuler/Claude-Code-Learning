using System.Net;
using System.Net.Http.Json;
using Checkout.Application.Carts;
using Checkout.Application.Orders;

namespace Checkout.IntegrationTests;

public class CheckoutFlowTests(CheckoutApiFactory factory) : IClassFixture<CheckoutApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Cart_to_paid_order_happy_path()
    {
        var keyboard = Guid.NewGuid();
        var mouse = Guid.NewGuid();
        var cart = await CreateCartAsync();

        await AddItemAsync(cart.Id, new AddCartItemRequest(keyboard, "Keyboard", 100m, 1));
        await AddItemAsync(cart.Id, new AddCartItemRequest(mouse, "Mouse", 20m, 2));
        var updated = await AddItemAsync(cart.Id, new AddCartItemRequest(mouse, "Mouse", 20m, 1));
        Assert.Equal(160m, updated.Total);

        var checkout = await _client.PostAsync($"/carts/{cart.Id}/checkout", null);
        Assert.Equal(HttpStatusCode.Created, checkout.StatusCode);
        var order = (await checkout.Content.ReadFromJsonAsync<OrderResponse>())!;
        Assert.Equal("Pending", order.Status);
        Assert.Equal(160m, order.Total);

        var pay = await _client.PostAsync($"/orders/{order.Id}/pay", null);
        pay.EnsureSuccessStatusCode();

        var fetched = await _client.GetFromJsonAsync<OrderResponse>($"/orders/{order.Id}");
        Assert.Equal("Paid", fetched!.Status);
        Assert.Equal(2, fetched.Items.Count);
    }

    [Fact]
    public async Task Checkout_empty_cart_returns_400()
    {
        var cart = await CreateCartAsync();

        var response = await _client.PostAsync($"/carts/{cart.Id}/checkout", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_unknown_order_returns_404()
    {
        var response = await _client.GetAsync($"/orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<CartResponse> CreateCartAsync()
    {
        var response = await _client.PostAsync("/carts", null);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<CartResponse>())!;
    }

    private async Task<CartResponse> AddItemAsync(Guid cartId, AddCartItemRequest request)
    {
        var response = await _client.PostAsJsonAsync($"/carts/{cartId}/items", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CartResponse>())!;
    }
}
