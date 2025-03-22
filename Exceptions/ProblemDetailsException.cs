using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// Exception with a ProblemDetails property. This
/// exception will be passed on as a response.
/// </summary>
public class ProblemDetailsException : Exception
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ProblemDetailsException()
    {
        Details = null;
    }
    
    /// <summary>
    /// Problem details
    /// </summary>
    public ProblemDetails? Details { get; set; }
}