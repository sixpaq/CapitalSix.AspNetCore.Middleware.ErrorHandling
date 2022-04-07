using System.Net;
using System.Text.Json;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// The Exception middleware will handle all uncaught exceptions
/// and will transform them in to an XML response with HTTP status
/// code 500.
///
/// It will guarantee that all exceptions will be handled equally.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IActionResultExecutor<ObjectResult> _executor;
    private readonly ExceptionMiddlewareOptions _exceptionMiddlewareOptions;
    private readonly XmlWriterSettings _writerSettings;
    private static readonly RouteData EmptyRouteData = new RouteData();
    private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

    /// <summary>
    /// This is the constructor of the Exception middleware
    /// </summary>
    /// <param name="next">This is the standard request delegate for the next middleware</param>
    /// <param name="exceptionMiddlewareOptions">Configuration settings for the middleware</param>
    /// <param name="executor">The action executor instance</param>
    /// <param name="logger">This is the logger</param>
    public ExceptionMiddleware(
        RequestDelegate next,
        IOptions<ExceptionMiddlewareOptions> exceptionMiddlewareOptions,
        IActionResultExecutor<ObjectResult> executor,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _exceptionMiddlewareOptions = exceptionMiddlewareOptions.Value;
        _executor = executor;
        _writerSettings = new XmlWriterSettings()
        {
            Async = true,
            OmitXmlDeclaration = true
        };
    }

    /// <summary>
    /// This Invoke method will handle any uncaught exception
    /// and transform it into a XML formed response with HTTP
    /// status code 500
    /// </summary>
    /// <param name="context">The HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ProblemDetails? problemDetails = null;

            var mapping = _exceptionMiddlewareOptions.Mappings
                .Where(m => ex.GetType().IsSubclassOf(m.Key))
                .Select(m => m.Value)
                .SingleOrDefault();
            if (mapping != null)
                problemDetails = mapping(ex);
            
            problemDetails ??= new ProblemDetails()
            {
                Status = 500,
                Title = ex.Message,
                Type = ex.GetType().Name
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json; application/json";
            context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
            
            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                problemDetails,
                problemDetails.GetType());

            _logger.LogError("{Message}", ex.Message);
            _logger.LogDebug("{StackTrace}", ex.StackTrace);
        }
    }
}
