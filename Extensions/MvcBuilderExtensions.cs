using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// This extension class provides a couple of extension
/// methods that will be used in the startup configuration
/// of each logical app.
/// </summary>
public static class MvcBuilderExtensions
{
    /// <summary>
    /// This extension method configures the use of the
    /// exception middleware. It configures the dependency
    /// injection, necessary for the middleware.
    /// </summary>
    /// <param name="builder">The MVC builder</param>
    /// <returns>The MVC builder</returns>
    public static IMvcBuilder AddExceptionMiddleware(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(opt =>
        {
            opt.InvalidModelStateResponseFactory = (context => throw new ActionModelStateException(context));
        });
        return builder;
    }
}
