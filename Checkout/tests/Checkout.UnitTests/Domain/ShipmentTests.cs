using Checkout.Domain.Common;
using Checkout.Domain.Shipments;

namespace Checkout.UnitTests.Domain;

public class ShipmentTests
{
    [Fact]
    public void Create_valid_input_normalizes_number_and_carrier()
    {
        var orderId = Guid.NewGuid();

        var shipment = Shipment.Create(orderId, "  shp-123456 ", " DHL ");

        Assert.Equal("SHP-123456", shipment.Number);
        Assert.Equal("DHL", shipment.Carrier);
        Assert.Equal(orderId, shipment.OrderId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("SHP-12345")]
    [InlineData("SHP-1234567")]
    [InlineData("XYZ-123456")]
    public void NormalizeNumber_malformed_number_throws(string? number)
    {
        Assert.Throws<DomainException>(() => Shipment.NormalizeNumber(number));
    }

    [Fact]
    public void Create_empty_order_id_throws()
    {
        Assert.Throws<DomainException>(() => Shipment.Create(Guid.Empty, "SHP-123456", "DHL"));
    }

    [Fact]
    public void Create_blank_carrier_throws()
    {
        Assert.Throws<DomainException>(() => Shipment.Create(Guid.NewGuid(), "SHP-123456", " "));
    }
}
