using System.Text.Json;
using System.Text.Json.Nodes;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Themes.Settings;

/// <summary>
/// Important: NEVER use this on a 
/// </summary>
public class ThemePartJsonConverter : JsonConverterBase<MagicThemePartSettings>
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
    private ThemePartJsonConverter() {}

    public static ThemePartJsonConverter GetNew() => new();

    public override void Write(Utf8JsonWriter writer, MagicThemePartSettings? part, JsonSerializerOptions options)
    {
        if (part == null || (part.Show == null && part.Configuration == null && part.Design == null))
        {
            writer.WriteNullValue();
            return;
        }

        // Copy options to remove this serializer, then serialize with default method
        JsonSerializer.Serialize(writer, part, GetOptionsWithoutThisConverter(options));
    }



    public override MagicThemePartSettings? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = JsonNode.Parse(ref reader);
        return x switch
        {
            null => null,
            JsonArray jArray => Dummy(),
            JsonValue jValue => ConvertValue(jValue),
            JsonObject jObject => ConvertObject(jObject, options),
            _ => Dummy(),
        };
    }

    private MagicThemePartSettings ConvertValue(JsonValue value)
    {
        if (value.TryGetValue<string>(out var str))
        {
            var asBool = IsBoolean(str);
            return asBool != null ? new(asBool.Value) : new(str);
        }

        if (value.TryGetValue<bool>(out var bln))
            return new(bln);

        return Dummy();
    }

    private bool? IsBoolean(string value)
    {
        if (!value.HasValue()) return null;
        if (value.EqInvariant(true.ToString())) return true;
        if (value.EqInvariant(false.ToString())) return false;
        return null;
    }

    private MagicThemePartSettings Dummy() => new()
    {
        Show = null,
        Design = null,
        Configuration = null,
    };
}
