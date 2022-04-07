using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

public class ProblemDetailsException : Exception
{
    public ProblemDetailsException()
    {
        Details = null;
    }
    
    public ProblemDetails? Details { get; set; }
}