using System.Net;
using System.Net.Http.Json;
using Marketplace.Application.Products;
using Marketplace.Application.Sellers;

namespace Marketplace.IntegrationTests;

public class ProductEndpointsTests(MarketplaceApiFactory factory) : IClassFixture<MarketplaceApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Create_seller_then_product_then_get_it_back()
    {
        var seller = await PostAsync<SellerResponse>("/sellers", new CreateSellerRequest("Acme", "sales@acme.test"));

        var created = await PostAsync<ProductResponse>("/products",
            new CreateProductRequest(seller.Id, "Mechanical Keyboard", 99.90m, "USD", 3));

        var fetched = await _client.GetFromJsonAsync<ProductResponse>($"/products/{created.Id}");
        Assert.Equal(created, fetched);

        var all = await _client.GetFromJsonAsync<List<ProductResponse>>("/products");
        Assert.Contains(all!, p => p.Id == created.Id);
    }

    [Fact]
    public async Task Get_unknown_product_returns_404()
    {
        var response = await _client.GetAsync($"/products/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_product_with_zero_price_returns_400()
    {
        var seller = await PostAsync<SellerResponse>("/sellers", new CreateSellerRequest("Zero", "zero@shop.test"));

        var response = await _client.PostAsJsonAsync("/products",
            new CreateProductRequest(seller.Id, "Free thing", 0m, "USD", 1));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<T> PostAsync<T>(string url, object body)
    {
        var response = await _client.PostAsJsonAsync(url, body);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }
}
