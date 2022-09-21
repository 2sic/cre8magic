using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// Important: NEVER use this on a 
/// </summary>
public class PairOnOffJsonConverter : JsonConverter<PairOnOff>
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
    private PairOnOffJsonConverter() {}

    public static PairOnOffJsonConverter GetNew() => new();

    public override void Write(Utf8JsonWriter writer, PairOnOff? pair, JsonSerializerOptions options)
    {
        if (pair?.On == null && pair?.Off == null)
        {
            writer.WriteNullValue();
            return;
        }

        // Copy options to remove this serializer, then serialize with default method
        JsonSerializer.Serialize(writer, pair, GetOptionsWithoutThisConverter(options));
    }

    private JsonSerializerOptions GetOptionsWithoutThisConverter(JsonSerializerOptions options)
    {
        // For performance reasons, only re-create the options without converter
        // If the input is different than last time
        if (_cachedDefaultOptions == options && _optionsWithoutConverter != null)
            return _optionsWithoutConverter;
        _cachedDefaultOptions = options;

        _optionsWithoutConverter = new(options);
        _optionsWithoutConverter.Converters.Remove(this);
        return _optionsWithoutConverter;
    }

    private JsonSerializerOptions? _cachedDefaultOptions;
    private JsonSerializerOptions? _optionsWithoutConverter;


    public override PairOnOff? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = JsonNode.Parse(ref reader);
        return x switch
        {
            null => null,
            JsonArray jArray => ArrayToPair(jArray),
            JsonValue jValue => new() { On = jValue.ToString() },
            JsonObject jObject => ObjectToPair(jObject, options),
            _ => new() { On = "error", Off = "error" },
        };
    }

    private PairOnOff? ArrayToPair(JsonArray jsonArray)
    {
        if (jsonArray.Count == 0) return null;
        return new()
        {
            On = jsonArray[0]?.ToString(),
            Off = jsonArray.Count > 1 ? jsonArray[1]?.ToString() : null
        };
    }

    private PairOnOff? ObjectToPair(JsonObject jsonObject, JsonSerializerOptions options) 
        => jsonObject.Deserialize<PairOnOff>(GetOptionsWithoutThisConverter(options));
}
