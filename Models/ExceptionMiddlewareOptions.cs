namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// This model contains the mappings for the exception middleware
/// </summary>
internal class ExceptionMiddlewareOptions
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ExceptionMiddlewareOptions()
    {
        Mappings = new ExceptionMiddlewareMappingDictionary();
        Mappings.Add<ProblemDetailsException>(ex => ex.Details);
    }
    
    public ExceptionMiddlewareMappingDictionary Mappings { get; }
}
