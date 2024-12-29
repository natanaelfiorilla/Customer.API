using Microsoft.OpenApi.Models;
using Taxdown.API.Middlewares;
using Taxdown.ApplicationServices;
using Taxdown.Infraestructure;

namespace Taxdown.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAwsServices(Configuration)
            .AddRepositories()
            .AddApplicationServices()
            .AddControllers();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "Taxdown Customer", 
                Version = "v1"
            });
            
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the endpoints. Add 'X-API-KEY: {your key}' to the headers",
                In = ParameterLocation.Header,
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxdown Customer V1");
        });

        app.UseMiddleware<ApiKeyMiddleware>();
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}