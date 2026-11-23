using Checkout.Application.Carts;
using Checkout.Application.Orders;
using Checkout.Application.Shipments;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CartService>();
        services.AddScoped<OrderService>();
        services.AddScoped<ShipmentService>();
        return services;
    }
}
