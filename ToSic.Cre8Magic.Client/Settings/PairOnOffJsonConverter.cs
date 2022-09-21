using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public class PairOnOffJsonConverter : JsonConverter<PairOnOff>
{
    public override void Write(Utf8JsonWriter writer, PairOnOff? pair, JsonSerializerOptions options)
    {
        if (pair?.On == null && pair?.Off == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WritePair(nameof(pair.On), pair.On, true);
        writer.WritePair(nameof(pair.Off), pair.Off, true);
        writer.WriteEndObject();
    }


    public override PairOnOff? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = JsonNode.Parse(ref reader);
        return x switch
        {
            null => null,
            JsonArray jArray => ArrayToPair(jArray),
            JsonValue jValue => new() { On = jValue.ToString() },
            JsonObject jObject => ObjectToPair(jObject),
            _ => new() { On = "error", Off = "error" },
        };
    }

    private PairOnOff? ArrayToPair(JsonArray jsonArray)
    {
        var parts = jsonArray.AsArray();
        if (parts.Count == 0) return null;
        return new()
        {
            On = parts[0]?.ToString(),
            Off = parts.Count > 1 ? parts[1]?.ToString() : null
        };
    }

    private PairOnOff? ObjectToPair(JsonObject jsonObject)
    {
        if (jsonObject == null) return null;
        jsonObject.TryGetPropertyValue(nameof(PairOnOff.On), out var on);
        jsonObject.TryGetPropertyValue(nameof(PairOnOff.Off), out var off);

        if (on == null && off == null) return null;

        return new(on?.ToString(), off?.ToString());
    }
}