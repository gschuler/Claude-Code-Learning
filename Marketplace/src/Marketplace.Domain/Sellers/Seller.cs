using Marketplace.Domain.Common;

namespace Marketplace.Domain.Sellers;

public sealed class Seller
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    private Seller() { } // EF Core

    public static Seller Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Seller name is required.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("A valid seller email is required.");

        return new Seller { Id = Guid.NewGuid(), Name = name.Trim(), Email = email.Trim() };
    }
}
