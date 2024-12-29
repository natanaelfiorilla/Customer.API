namespace Taxdown.API.Middlewares;

public class ApiKeyMiddleware
{
    private const string API_KEY_HEADER_NAME = "X-API-KEY";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1) Skip API key check for swagger / or other paths
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.Equals("/index.html"))
        {
            // Just call the next middleware in the pipeline
            await _next(context);
            return;
        }
        
        // 2) Check if the request includes the required header
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // 3) Compare extracted key with the one in appsettings.json
        var apiKey = _configuration.GetValue<string>("ApiSettings:ApiKey");
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid API Key provided.");
            return;
        }

        // 4) If valid, continue processing
        await _next(context);
    }
} 