using Microsoft.AspNetCore.Builder;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// Extension class for IApplicationBuilder instances
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// This extension will register the ExceptionMiddleware
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance</param>
    /// <returns>The IApplicationBuilder instance</returns>
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}