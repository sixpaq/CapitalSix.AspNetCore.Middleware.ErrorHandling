using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// This dictionary constructs the middleware mapping
/// </summary>
internal class ExceptionMiddlewareMappingDictionary : Dictionary<Type, Func<Exception, ProblemDetails?>>
{
    /// <summary>
    /// This method adds a mapping for the middleware
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="mapping"></param>
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