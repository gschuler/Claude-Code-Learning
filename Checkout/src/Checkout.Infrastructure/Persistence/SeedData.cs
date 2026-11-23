using Checkout.Domain.Orders;

namespace Checkout.Infrastructure.Persistence;

/// <summary>
/// Sample data inserted by EF Core (<c>HasData</c>) when the database is created.
/// Product ids/names/prices match the Marketplace seed data so both projects tell the same story.
/// </summary>
internal static class SeedData
{
    // Products copied from Marketplace's seed (ids b1000000-...).
    private static readonly Guid Keyboard = Guid.Parse("b1000000-0000-0000-0000-000000000001");
    private static readonly Guid Mouse = Guid.Parse("b1000000-0000-0000-0000-000000000002");
    private static readonly Guid Monitor = Guid.Parse("b1000000-0000-0000-0000-000000000003");
    private static readonly Guid CoffeeMaker = Guid.Parse("b1000000-0000-0000-0000-000000000005");
    private static readonly Guid CleanArchitectureBook = Guid.Parse("b1000000-0000-0000-0000-000000000007");

    // Carts: one still open, two already checked out (each one produced an order).
    public static readonly Guid OpenCart = Guid.Parse("c1000000-0000-0000-0000-000000000001");
    public static readonly Guid PaidCart = Guid.Parse("c1000000-0000-0000-0000-000000000002");
    public static readonly Guid PendingCart = Guid.Parse("c1000000-0000-0000-0000-000000000003");

    public static readonly Guid PaidOrder = Guid.Parse("d1000000-0000-0000-0000-000000000001");
    public static readonly Guid PendingOrder = Guid.Parse("d1000000-0000-0000-0000-000000000002");

    public static readonly object[] Carts =
    [
        new { Id = OpenCart, CreatedAtUtc = new DateTime(2026, 11, 20, 14, 0, 0, DateTimeKind.Utc), IsCheckedOut = false },
        new { Id = PaidCart, CreatedAtUtc = new DateTime(2026, 11, 18, 9, 30, 0, DateTimeKind.Utc), IsCheckedOut = true },
        new { Id = PendingCart, CreatedAtUtc = new DateTime(2026, 11, 19, 17, 45, 0, DateTimeKind.Utc), IsCheckedOut = true }
    ];

    public static readonly object[] CartItems =
    [
        new { CartId = OpenCart, ProductId = Keyboard, ProductName = "Mechanical Keyboard", UnitPrice = 349.90m, Quantity = 1 },
        new { CartId = OpenCart, ProductId = Mouse, ProductName = "Wireless Mouse", UnitPrice = 129.90m, Quantity = 2 },

        new { CartId = PaidCart, ProductId = Monitor, ProductName = "27\" Monitor", UnitPrice = 1499.00m, Quantity = 1 },
        new { CartId = PaidCart, ProductId = CleanArchitectureBook, ProductName = "Clean Architecture (book)", UnitPrice = 159.00m, Quantity = 1 },

        new { CartId = PendingCart, ProductId = CoffeeMaker, ProductName = "Coffee Maker", UnitPrice = 279.00m, Quantity = 1 }
    ];

    public static readonly object[] Orders =
    [
        new { Id = PaidOrder, CartId = PaidCart, Status = OrderStatus.Paid, CreatedAtUtc = new DateTime(2026, 11, 18, 9, 40, 0, DateTimeKind.Utc) },
        new { Id = PendingOrder, CartId = PendingCart, Status = OrderStatus.Pending, CreatedAtUtc = new DateTime(2026, 11, 19, 18, 0, 0, DateTimeKind.Utc) }
    ];

    public static readonly object[] OrderItems =
    [
        new { OrderId = PaidOrder, ProductId = Monitor, ProductName = "27\" Monitor", UnitPrice = 1499.00m, Quantity = 1 },
        new { OrderId = PaidOrder, ProductId = CleanArchitectureBook, ProductName = "Clean Architecture (book)", UnitPrice = 159.00m, Quantity = 1 },

        new { OrderId = PendingOrder, ProductId = CoffeeMaker, ProductName = "Coffee Maker", UnitPrice = 279.00m, Quantity = 1 }
    ];
}
