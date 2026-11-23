using Marketplace.Application.Products;
using Marketplace.Application.Sellers;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<SellerService>();
        services.AddScoped<ProductService>();
        return services;
    }
}
