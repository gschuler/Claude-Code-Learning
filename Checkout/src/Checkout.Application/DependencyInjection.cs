using Checkout.Application.Carts;
using Checkout.Application.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CartService>();
        services.AddScoped<OrderService>();
        return services;
    }
}
