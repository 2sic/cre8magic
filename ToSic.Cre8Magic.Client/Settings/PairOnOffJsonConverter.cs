using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using ToSic.Cre8magic.Client.Settings.Json;

namespace ToSic.Cre8magic.Client.Settings;

/// <summary>
/// Important: NEVER use this on a 
/// </summary>
public class PairOnOffJsonConverter : JsonConverterBase<PairOnOff>
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
    private PairOnOffJsonConverter(ILogger logger) : base(logger) 
    {}

    public static PairOnOffJsonConverter GetNew(ILogger logger) => new(logger);

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



    public override PairOnOff? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Logger.LogInformation($"cre8magic# Reading PairOnOffJsonConverter / {typeToConvert}.");
        var x = JsonNode.Parse(ref reader);
        return x switch
        {
            null => null,
            JsonArray jArray => ConvertArray(jArray),
            JsonValue jValue => new() { On = jValue.ToString() },
            JsonObject jObject => ConvertObject(jObject, GetOptionsWithoutThisConverter(options)),
            _ => new() { On = "error", Off = "error" },
        };
    }

    private PairOnOff? ConvertArray(JsonArray jsonArray)
    {
        if (jsonArray.Count == 0) return null;
        return new()
        {
            On = jsonArray[0]?.ToString(),
            Off = jsonArray.Count > 1 ? jsonArray[1]?.ToString() : null
        };
    }
}
