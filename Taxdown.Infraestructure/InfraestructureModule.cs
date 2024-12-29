using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taxdown.DomainServices;
using Taxdown.Infraestructure.Repositories;

namespace Taxdown.Infraestructure;

public static class InfraestructureModule
{
    public static IServiceCollection AddAwsServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();
        
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        return services;
    }
}