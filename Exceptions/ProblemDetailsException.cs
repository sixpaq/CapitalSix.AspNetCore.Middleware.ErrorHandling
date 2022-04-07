using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace AspNetCore.Middleware.ErrorHandling;

public class ProblemDetailsException : Exception
{
    public ProblemDetailsException()
    {
        Details = null;
    }
    
    public ProblemDetails? Details { get; set; }
}