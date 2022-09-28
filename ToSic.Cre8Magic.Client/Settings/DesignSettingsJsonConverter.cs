using System.Text.Json;
using System.Text.Json.Nodes;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// Important: NEVER use this on a 
/// </summary>
public class DesignSettingsJsonConverter<T> : JsonConverterBase<T> where T : DesignSetting, new()
{
    /// <summary>
    /// Private constructor to prevent use in attributes.
    /// So this is NOT allowed:
    /// [JsonConverter(typeof(PairOnOffJsonConverter))]
    ///
    /// ...because the converter must be enabled sometimes,
    /// but removed at other times to use default conversion.
    /// That is only possible if it's not used in a POCO attribute, but added in the serializer options.
    /// </summary>
    private DesignSettingsJsonConverter() {}

    public static DesignSettingsJsonConverter<T> GetNew() => new();

    public override void Write(Utf8JsonWriter writer, T? pair, JsonSerializerOptions options) =>
        // Copy options to remove this serializer, then serialize with default method
        JsonSerializer.Serialize(writer, pair, GetOptionsWithoutThisConverter(options));


    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonNode = JsonNode.Parse(ref reader);

        const string errArray = "Error unexpected data - array instead of string or object";
        return jsonNode switch
        {
            null => null,
            JsonArray _ => ConvertValue(errArray),
            JsonValue jValue => ConvertValue(jValue.ToString()),
            JsonObject jObject => ConvertObject(jObject, options),
            _ => null,
        };
    }

    private T ConvertValue(string value) => new() { Classes = value, Value = value };
}
