using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

public class ExceptionMiddlewareOptions
{
    public ExceptionMiddlewareOptions()
    {
        Mappings = new ExceptionMiddlewareMappingDictionary();
        Mappings.Add<ProblemDetailsException>(ex => ex.Details);
    }
    
    public ExceptionMiddlewareMappingDictionary Mappings { get; }
}

public class ExceptionMiddlewareMappingDictionary : Dictionary<Type, Func<Exception, ProblemDetails?>>
{
    public void Add<TException>(Func<TException, ProblemDetails?> mapping) where TException : Exception
    {
        Add(typeof(TException), ex =>
        {
            if (ex is TException exception)
                return mapping(exception);
            return null;
        });
    }
}