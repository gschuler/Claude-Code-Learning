using System.Text.RegularExpressions;
using Checkout.Domain.Common;

namespace Checkout.Domain.Shipments;

public sealed partial class Shipment
{
    public Guid Id { get; private set; }

    /// <summary>Human-friendly tracking number, e.g. <c>SHP-100001</c>. Unique.</summary>
    public string Number { get; private set; } = null!;

    public Guid OrderId { get; private set; }
    public string Carrier { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }

    private Shipment() { } // EF Core

    public static Shipment Create(Guid orderId, string number, string carrier)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order id is required.");
        if (string.IsNullOrWhiteSpace(carrier))
            throw new DomainException("Carrier is required.");

        return new Shipment
        {
            Id = Guid.NewGuid(),
            Number = NormalizeNumber(number),
            OrderId = orderId,
            Carrier = carrier.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Trims and upper-cases a shipment number and checks its format (<c>SHP-</c> followed by 6 digits).
    /// </summary>
    public static string NormalizeNumber(string? number)
    {
        var normalized = number?.Trim().ToUpperInvariant() ?? string.Empty;
        if (!NumberFormat().IsMatch(normalized))
            throw new DomainException($"'{number}' is not a valid shipment number (expected format SHP-000000).");
        return normalized;
    }

    [GeneratedRegex(@"^SHP-\d{6}$")]
    private static partial Regex NumberFormat();
}
