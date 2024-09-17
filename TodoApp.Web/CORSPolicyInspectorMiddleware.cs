using Microsoft.AspNetCore.Cors.Infrastructure;

namespace TodoApp.Web;

public class CorsPolicyInspectorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICorsPolicyProvider _corsPolicyProvider;
    private readonly ILogger<CorsPolicyInspectorMiddleware> _logger;

    public CorsPolicyInspectorMiddleware(RequestDelegate next, ICorsPolicyProvider corsPolicyProvider, ILogger<CorsPolicyInspectorMiddleware> logger)
    {
        _next = next;
        _corsPolicyProvider = corsPolicyProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Retrieve the CORS policy
        var corsPolicy = await _corsPolicyProvider.GetPolicyAsync(context, "_AllowSpecificOrigins");

        if (corsPolicy != null)
        {
            _logger.LogInformation("Allowed Origins:");
            foreach (var origin in corsPolicy.Origins)
            {
                _logger.LogInformation(origin);
            }
        }
        else
        {
            _logger.LogInformation("CORS policy not found.");
        }

        // Continue with the next middleware
        await _next(context);
    }
}

// Extension method for adding the middleware to the pipeline
public static class CorsPolicyInspectorMiddlewareExtensions
{
    public static IApplicationBuilder UseCorsPolicyInspector(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsPolicyInspectorMiddleware>();
    }
}