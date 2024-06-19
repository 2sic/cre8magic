using System.Text.Json;

namespace ToSic.Cre8magic.Client;

internal static class Utf8JsonWriterExtensions
{
    public static void WritePair(this Utf8JsonWriter writer, string name, string? value, bool skipIfNull = false)
    {
        if (skipIfNull && value == null) return;
        writer.WritePropertyName(name);
        writer.WriteStringValue(value);
    }
}