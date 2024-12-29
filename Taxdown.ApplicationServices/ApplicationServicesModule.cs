using Microsoft.Extensions.DependencyInjection;
using Taxdown.ApplicationServices.Services;

namespace Taxdown.ApplicationServices;

public static class ApplicationServicesModule
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        
        return services;
    }
}