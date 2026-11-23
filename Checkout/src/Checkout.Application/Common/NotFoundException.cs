namespace Checkout.Application.Common;

/// <summary>The key is usually a <see cref="Guid"/> id, but can be any natural key (e.g. a shipment number).</summary>
public sealed class NotFoundException(string resource, object key)
    : Exception($"{resource} '{key}' was not found.");
