using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace AspNetCore.Middleware.ErrorHandling;

[JsonConverter(typeof(ActionModelStateProblemDetailsJsonConverter))]
public class ActionModelStateProblemDetails : ProblemDetails
{
    [JsonPropertyName("errors")]
    public Dictionary<string, string>? Errors { get; set; } = null;
}