using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// This exception is thrown when a model does not validate
/// </summary>
public class ActionModelStateException : ProblemDetailsException
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context"></param>
    public ActionModelStateException(ActionContext context)
        : base()
    {
        Details = new ActionModelStateProblemDetails()
        {
            Status = (int) HttpStatusCode.BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
        
        var errors = new Dictionary<string, string>();
        
        foreach (var stateEntry in context.ModelState)
        {
            foreach (var modelError in stateEntry.Value.Errors)
            {
                errors.Add(stateEntry.Key, modelError.ErrorMessage);
            }
        }

        ((Details as ActionModelStateProblemDetails)!).Errors = errors;
    }
}