using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// Model for returning the invalidated model state
/// </summary>
[JsonConverter(typeof(ActionModelStateProblemDetailsJsonConverter))]
public class ActionModelStateProblemDetails : ProblemDetails
{
    /// <summary>
    /// A collection containing all model state errors
    /// </summary>
    [JsonPropertyName("errors")]
    public Dictionary<string, string>? Errors { get; set; } = null;
}