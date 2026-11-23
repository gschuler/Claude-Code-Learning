using System.Net;
using System.Net.Http.Json;
using Checkout.Application.Shipments;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.IntegrationTests;

public class ShipmentEndpointsTests(CheckoutApiFactory factory) : IClassFixture<CheckoutApiFactory>
{
    // From Infrastructure/Persistence/SeedData.cs: the shipment of the seeded paid order.
    private const string SeededNumber = "SHP-100001";
    private static readonly Guid SeededShipmentId = Guid.Parse("e1000000-0000-0000-0000-000000000001");
    private static readonly Guid SeededPaidOrderId = Guid.Parse("d1000000-0000-0000-0000-000000000001");

    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetByNumber_existing_shipment_returns_200_with_shipment()
    {
        var response = await _client.GetAsync($"/shipments/by-number/{SeededNumber}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var shipment = (await response.Content.ReadFromJsonAsync<ShipmentResponse>())!;
        Assert.Equal(SeededShipmentId, shipment.Id);
        Assert.Equal(SeededNumber, shipment.Number);
        Assert.Equal(SeededPaidOrderId, shipment.OrderId);
        Assert.Equal("Correios", shipment.Carrier);
    }

    [Fact]
    public async Task GetByNumber_lowercase_number_returns_same_shipment()
    {
        var response = await _client.GetAsync("/shipments/by-number/shp-100001");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var shipment = (await response.Content.ReadFromJsonAsync<ShipmentResponse>())!;
        Assert.Equal(SeededShipmentId, shipment.Id);
    }

    [Fact]
    public async Task GetByNumber_unknown_number_returns_404_problem_details()
    {
        var response = await _client.GetAsync("/shipments/by-number/SHP-999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var problem = (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;
        Assert.Equal(404, problem.Status);
        Assert.Equal("Shipment 'SHP-999999' was not found.", problem.Detail);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("SHP-12")]
    [InlineData("ORD-100001")]
    [InlineData("SHP-1000011")]
    public async Task GetByNumber_malformed_number_returns_400_problem_details(string number)
    {
        var response = await _client.GetAsync($"/shipments/by-number/{number}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;
        Assert.Equal(400, problem.Status);
        Assert.Contains("not a valid shipment number", problem.Detail);
    }

    [Fact]
    public async Task GetById_existing_shipment_returns_200_with_shipment()
    {
        var shipment = await _client.GetFromJsonAsync<ShipmentResponse>($"/shipments/{SeededShipmentId}");

        Assert.Equal(SeededNumber, shipment!.Number);
    }

    [Fact]
    public async Task GetById_unknown_shipment_returns_404()
    {
        var response = await _client.GetAsync($"/shipments/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
