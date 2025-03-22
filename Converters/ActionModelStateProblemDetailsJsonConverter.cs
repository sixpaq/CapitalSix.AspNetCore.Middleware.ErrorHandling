using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CapitalSix.AspNetCore.Middleware.ErrorHandling;

/// <summary>
/// 
/// </summary>
internal sealed class ActionModelStateProblemDetailsJsonConverter : JsonConverter<ActionModelStateProblemDetails>
{
    private static readonly JsonEncodedText _type = JsonEncodedText.Encode("type");
    private static readonly JsonEncodedText _title = JsonEncodedText.Encode("title");
    private static readonly JsonEncodedText _status = JsonEncodedText.Encode("status");
    private static readonly JsonEncodedText _detail = JsonEncodedText.Encode("detail");
    private static readonly JsonEncodedText _instance = JsonEncodedText.Encode("instance");
    private static readonly JsonEncodedText _errors = JsonEncodedText.Encode("errors");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public override ActionModelStateProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var problemDetails = new ActionModelStateProblemDetails();

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Unexcepted end when reading JSON.");
        }

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            ReadValue(ref reader, problemDetails, options);
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Unexcepted end when reading JSON.");
        }

        return problemDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, ActionModelStateProblemDetails value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProblemDetails(writer, value, options);
        writer.WriteEndObject();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    internal static void ReadValue(ref Utf8JsonReader reader, ProblemDetails value, JsonSerializerOptions options)
    {
        if (TryReadStringProperty(ref reader, _type, out var propertyValue))
        {
            value.Type = propertyValue;
        }
        else if (TryReadStringProperty(ref reader, _title, out propertyValue))
        {
            value.Title = propertyValue;
        }
        else if (TryReadStringProperty(ref reader, _detail, out propertyValue))
        {
            value.Detail = propertyValue;
        }
        else if (TryReadStringProperty(ref reader, _instance, out propertyValue))
        {
            value.Instance = propertyValue;
        }
        else if (reader.ValueTextEquals(_status.EncodedUtf8Bytes))
        {
            reader.Read();
            if (reader.TokenType == JsonTokenType.Null)
            {
                // Nothing to do here.
            }
            else
            {
                value.Status = reader.GetInt32();
            }
        }
        else
        {
            var key = reader.GetString()!;
            reader.Read();
            value.Extensions[key] = JsonSerializer.Deserialize(ref reader, typeof(object), options);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static bool TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out string? value)
    {
        if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
        {
            value = default;
            return false;
        }

        reader.Read();
        value = reader.GetString()!;
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    internal static void WriteProblemDetails(Utf8JsonWriter writer, ActionModelStateProblemDetails value, JsonSerializerOptions options)
    {
        if (value.Type != null)
        {
            writer.WriteString(_type, value.Type);
        }

        if (value.Title != null)
        {
            writer.WriteString(_title, value.Title);
        }

        if (value.Status != null)
        {
            writer.WriteNumber(_status, value.Status.Value);
        }

        if (value.Detail != null)
        {
            writer.WriteString(_detail, value.Detail);
        }

        if (value.Instance != null)
        {
            writer.WriteString(_instance, value.Instance);
        }

        foreach (var kvp in value.Extensions)
        {
            writer.WritePropertyName(kvp.Key);
            JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(object), options);
        }

        if (value.Errors != null && value.Errors.Any())
        {
            writer.WriteStartObject(_errors);
            foreach (var error in value.Errors)
            {
                var key = JsonEncodedText.Encode(error.Key);
                writer.WriteString(key, error.Value);
            }
            writer.WriteEndObject();
        }
    }
}