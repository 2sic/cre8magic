using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace ToSic.Cre8Magic.Client.Settings.Json;

/// <summary>
/// Inspired by https://github.com/dotnet/runtime/issues/31433
/// </summary>
internal class JsonMerger
{
    public static JsonSerializerOptions GetNewOptionsForPreMerge(ILogger logger) => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters =
        {
            PairOnOffJsonConverter.GetNew(logger),
            DesignSettingsJsonConverter<DesignSetting>.GetNew(logger),
            // DesignSettingsJsonConverter<DesignSettingActive>.GetNew(),
            DesignSettingsJsonConverter<MagicMenuDesign>.GetNew(logger),
            // DesignSettingsJsonConverter<MagicContainerDesignSettingsItem>.GetNew(),
            ThemePartJsonConverter.GetNew(logger),
        },
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    public static TType Clone<TType>(TType original)
    {
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<TType>(json);
        return result!;
    }

    public static TType Merge<TType>(TType priority, TType fallback, ILogger logger,
        Func<string, string>? optionalProcessing = null)
    {
        var priorityJson = JsonSerializer.Serialize(priority, GetNewOptionsForPreMerge(logger));
        var lessJson = fallback == null ? null : JsonSerializer.Serialize(fallback, GetNewOptionsForPreMerge(logger));
        var merged = lessJson == null ? priorityJson : Merge(priorityJson, lessJson);
        var processed = optionalProcessing?.Invoke(merged) ?? merged;
        var result = JsonSerializer.Deserialize<TType>(processed, GetNewOptionsForPreMerge(logger));
        return result!;
    }

    public static string Merge(string priority, string secondary)
    {
        var outputBuffer = new ArrayBufferWriter<byte>();

        using (JsonDocument jDoc1 = JsonDocument.Parse(secondary))
        using (JsonDocument jDoc2 = JsonDocument.Parse(priority))
        using (var jsonWriter = new Utf8JsonWriter(outputBuffer, new() { Indented = true }))
        {
            JsonElement root1 = jDoc1.RootElement;
            JsonElement root2 = jDoc2.RootElement;

            if (root1.ValueKind != JsonValueKind.Array && root1.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException($"The original JSON document to merge new content into must be a container type. Instead it is {root1.ValueKind}.");
            }

            if (root1.ValueKind != root2.ValueKind)
            {
                return secondary;
            }

            if (root1.ValueKind == JsonValueKind.Array)
            {
                MergeArrays(jsonWriter, root1, root2);
            }
            else
            {
                MergeObjects(jsonWriter, root1, root2);
            }
        }

        return Encoding.UTF8.GetString(outputBuffer.WrittenSpan);
    }

    private static void MergeObjects(Utf8JsonWriter jsonWriter, JsonElement root1, JsonElement root2)
    {
        System.Diagnostics.Debug.Assert(root1.ValueKind == JsonValueKind.Object);
        System.Diagnostics.Debug.Assert(root2.ValueKind == JsonValueKind.Object);

        jsonWriter.WriteStartObject();

        // Write all the properties of the first document.
        // If a property exists in both documents, either:
        // * Merge them, if the value kinds match (e.g. both are objects or arrays),
        // * Completely override the value of the first with the one from the second, if the value kind mismatches (e.g. one is object, while the other is an array or string),
        // * Or favor the value of the first (regardless of what it may be), if the second one is null (i.e. don't override the first).
        foreach (JsonProperty property in root1.EnumerateObject())
        {
            string propertyName = property.Name;

            JsonValueKind newValueKind;

            if (root2.TryGetProperty(propertyName, out JsonElement newValue) && (newValueKind = newValue.ValueKind) != JsonValueKind.Null)
            {
                jsonWriter.WritePropertyName(propertyName);

                JsonElement originalValue = property.Value;
                JsonValueKind originalValueKind = originalValue.ValueKind;

                if (newValueKind == JsonValueKind.Object && originalValueKind == JsonValueKind.Object)
                {
                    MergeObjects(jsonWriter, originalValue, newValue); // Recursive call
                }
                else if (newValueKind == JsonValueKind.Array && originalValueKind == JsonValueKind.Array)
                {
                    MergeArrays(jsonWriter, originalValue, newValue);
                }
                else
                {
                    newValue.WriteTo(jsonWriter);
                }
            }
            else
            {
                property.WriteTo(jsonWriter);
            }
        }

        // Write all the properties of the second document that are unique to it.
        foreach (JsonProperty property in root2.EnumerateObject())
        {
            if (!root1.TryGetProperty(property.Name, out _))
            {
                property.WriteTo(jsonWriter);
            }
        }

        jsonWriter.WriteEndObject();
    }

    private static void MergeArrays(Utf8JsonWriter jsonWriter, JsonElement root1, JsonElement root2)
    {
        System.Diagnostics.Debug.Assert(root1.ValueKind == JsonValueKind.Array);
        System.Diagnostics.Debug.Assert(root2.ValueKind == JsonValueKind.Array);

        jsonWriter.WriteStartArray();

        // Write all the elements from both JSON arrays
        foreach (JsonElement element in root1.EnumerateArray())
        {
            element.WriteTo(jsonWriter);
        }
        foreach (JsonElement element in root2.EnumerateArray())
        {
            element.WriteTo(jsonWriter);
        }

        jsonWriter.WriteEndArray();
    }
    //public static string SimpleObjectMergeWithNullHandling(string originalJson, string newContent)
    //{
    //    var outputBuffer = new ArrayBufferWriter<byte>();

    //    using (JsonDocument jDoc1 = JsonDocument.Parse(originalJson))
    //    using (JsonDocument jDoc2 = JsonDocument.Parse(newContent))
    //    using (var jsonWriter = new Utf8JsonWriter(outputBuffer, new JsonWriterOptions { Indented = true }))
    //    {
    //        JsonElement root1 = jDoc1.RootElement;
    //        JsonElement root2 = jDoc2.RootElement;

    //        // Assuming both JSON strings are single JSON objects (i.e. {...})
    //        Debug.Assert(root1.ValueKind == JsonValueKind.Object);
    //        Debug.Assert(root2.ValueKind == JsonValueKind.Object);

    //        jsonWriter.WriteStartObject();

    //        // Write all the properties of the first document that don't conflict with the second
    //        // Or if the second is overriding it with null, favor the property in the first.
    //        foreach (JsonProperty property in root1.EnumerateObject())
    //        {
    //            if (!root2.TryGetProperty(property.Name, out JsonElement newValue) || newValue.ValueKind == JsonValueKind.Null)
    //            {
    //                property.WriteTo(jsonWriter);
    //            }
    //        }

    //        // Write all the properties of the second document (including those that are duplicates which were skipped earlier)
    //        // The property values of the second document completely override the values of the first, unless they are null in the second.
    //        foreach (JsonProperty property in root2.EnumerateObject())
    //        {
    //            // Don't write null values, unless they are unique to the second document
    //            if (property.Value.ValueKind != JsonValueKind.Null || !root1.TryGetProperty(property.Name, out _))
    //            {
    //                property.WriteTo(jsonWriter);
    //            }
    //        }

    //        jsonWriter.WriteEndObject();
    //    }

    //    return Encoding.UTF8.GetString(outputBuffer.WrittenSpan);
    //}
}