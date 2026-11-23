using Marketplace.Domain.Common;
using Marketplace.Domain.Products;

namespace Marketplace.UnitTests.Domain;

public class ProductTests
{
    private static readonly Guid SellerId = Guid.NewGuid();

    [Fact]
    public void Create_with_valid_data_sets_properties()
    {
        var product = Product.Create(SellerId, "  Keyboard ", new Money(100m, "usd"), 5);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("Keyboard", product.Name);
        Assert.Equal(new Money(100m, "USD"), product.Price);
        Assert.Equal(5, product.Stock);
    }

    [Fact]
    public void Create_with_zero_price_throws()
    {
        Assert.Throws<DomainException>(() => Product.Create(SellerId, "Keyboard", new Money(0m, "USD"), 5));
    }

    [Fact]
    public void Create_with_negative_stock_throws()
    {
        Assert.Throws<DomainException>(() => Product.Create(SellerId, "Keyboard", new Money(10m, "USD"), -1));
    }

    [Fact]
    public void DecreaseStock_reduces_available_stock()
    {
        var product = Product.Create(SellerId, "Keyboard", new Money(10m, "USD"), 5);

        product.DecreaseStock(3);

        Assert.Equal(2, product.Stock);
    }

    [Fact]
    public void DecreaseStock_beyond_available_throws()
    {
        var product = Product.Create(SellerId, "Keyboard", new Money(10m, "USD"), 2);

        Assert.Throws<DomainException>(() => product.DecreaseStock(3));
    }

    [Theory]
    [InlineData("")]
    [InlineData("US")]
    [InlineData("DOLLAR")]
    public void Money_with_invalid_currency_throws(string currency)
    {
        Assert.Throws<DomainException>(() => new Money(10m, currency));
    }
}
