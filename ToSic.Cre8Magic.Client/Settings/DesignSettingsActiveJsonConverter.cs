using System.Text.Json;
using System.Text.Json.Nodes;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// Important: NEVER use this on a 
/// </summary>
public class DesignSettingsActiveJsonConverter : JsonConverterBase<DesignSettingActive>
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
    private DesignSettingsActiveJsonConverter() {}

    public static DesignSettingsActiveJsonConverter GetNew() => new();

    public override void Write(Utf8JsonWriter writer, DesignSettingActive? pair, JsonSerializerOptions options) =>
        // Copy options to remove this serializer, then serialize with default method
        JsonSerializer.Serialize(writer, pair, GetOptionsWithoutThisConverter(options));


    public override DesignSettingActive? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = JsonNode.Parse(ref reader);

        return x switch
        {
            null => null,
            JsonArray jArray => null, //ConvertArray(jArray),
            JsonValue jValue =>  new() { Classes = jValue.ToString() },
            JsonObject jObject => ConvertObject(jObject, options),
            _ => null,
        };
    }

    //private PairOnOff? ConvertArray(JsonArray jsonArray)
    //{
    //    if (jsonArray.Count == 0) return null;
    //    return new()
    //    {
    //        On = jsonArray[0]?.ToString(),
    //        Off = jsonArray.Count > 1 ? jsonArray[1]?.ToString() : null
    //    };
    //}
}
