namespace Marketplace.Infrastructure.Persistence;

/// <summary>
/// Sample data inserted by EF Core (<c>HasData</c>) when the database is created.
/// Ids are fixed so they can be reused in .http files and by the Checkout project.
/// </summary>
internal static class SeedData
{
    public static class Sellers
    {
        public static readonly Guid TechStore = Guid.Parse("a1000000-0000-0000-0000-000000000001");
        public static readonly Guid HomeGoods = Guid.Parse("a1000000-0000-0000-0000-000000000002");
        public static readonly Guid BookNook = Guid.Parse("a1000000-0000-0000-0000-000000000003");

        public static readonly object[] Rows =
        [
            new { Id = TechStore, Name = "Tech Store", Email = "contact@techstore.test" },
            new { Id = HomeGoods, Name = "Home Goods", Email = "hello@homegoods.test" },
            new { Id = BookNook, Name = "Book Nook", Email = "books@booknook.test" }
        ];
    }

    public static class Products
    {
        // (Id, SellerId, Name, Price, Currency, Stock)
        public static readonly (Guid Id, Guid SellerId, string Name, decimal Price, string Currency, int Stock)[] All =
        [
            (Guid.Parse("b1000000-0000-0000-0000-000000000001"), Sellers.TechStore, "Mechanical Keyboard", 349.90m, "BRL", 25),
            (Guid.Parse("b1000000-0000-0000-0000-000000000002"), Sellers.TechStore, "Wireless Mouse", 129.90m, "BRL", 60),
            (Guid.Parse("b1000000-0000-0000-0000-000000000003"), Sellers.TechStore, "27\" Monitor", 1499.00m, "BRL", 8),
            (Guid.Parse("b1000000-0000-0000-0000-000000000004"), Sellers.TechStore, "USB-C Hub", 199.00m, "BRL", 0),
            (Guid.Parse("b1000000-0000-0000-0000-000000000005"), Sellers.HomeGoods, "Coffee Maker", 279.00m, "BRL", 15),
            (Guid.Parse("b1000000-0000-0000-0000-000000000006"), Sellers.HomeGoods, "Desk Lamp", 89.90m, "BRL", 40),
            (Guid.Parse("b1000000-0000-0000-0000-000000000007"), Sellers.BookNook, "Clean Architecture (book)", 159.00m, "BRL", 12),
            (Guid.Parse("b1000000-0000-0000-0000-000000000008"), Sellers.BookNook, "Domain-Driven Design (book)", 219.00m, "BRL", 5)
        ];

        public static object[] Rows => All
            .Select(p => (object)new { p.Id, p.SellerId, p.Name, p.Stock })
            .ToArray();

        // Owned Money value object: keyed by the owner's shadow FK "ProductId".
        public static object[] PriceRows => All
            .Select(p => (object)new { ProductId = p.Id, Amount = p.Price, p.Currency })
            .ToArray();
    }
}
