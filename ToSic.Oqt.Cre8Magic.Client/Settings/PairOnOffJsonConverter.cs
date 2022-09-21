using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public class PairOnOffJsonConverter : JsonConverter<PairOnOff>
{
    public override void Write(Utf8JsonWriter writer, PairOnOff? pair, JsonSerializerOptions options)
    {
        if (pair == null)
            writer.WriteNullValue();
        else if (pair.Off == null)
            writer.WriteStringValue(pair.On);
        else
        {
            writer.WriteStartArray();
            writer.WriteStringValue(pair.On);
            writer.WriteStringValue(pair.Off);
            writer.WriteEndArray();
        }
    }


    public override PairOnOff? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = JsonNode.Parse(ref reader);
        return x switch
        {
            null => null,
            JsonArray jArray => ArrayToPair(jArray),
            JsonValue jValue => new() { On = jValue.ToString() },
            JsonObject jObject => null,
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
}