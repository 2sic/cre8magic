using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace ToSic.Cre8magic.Client.Settings.Json;

public abstract class JsonConverterBase<T> : JsonConverter<T>
{
    public ILogger Logger { get; }

    protected JsonConverterBase(ILogger logger) => Logger = logger;

    private static readonly ConditionalWeakTable<JsonConverterBase<T>, BoxedBool> ConverterFlags = new();

    private bool IsInsideConverter
    {
        get => ConverterFlags.GetValue(this, _ => new BoxedBool()).Value;
        set => ConverterFlags.GetOrCreateValue(this).Value = value;
    }

    private class BoxedBool
    {
        public bool Value;
    }

    protected JsonSerializerOptions GetOptionsWithoutThisConverter(JsonSerializerOptions options)
    {
        if (IsInsideConverter)
        {
            Logger.LogInformation("cre8magic# Already inside converter {Converter}", this);
            return options; // Return the original options if we're already inside the converter
        }

        JsonSerializerOptions optionsWithoutConverter = new(options);
        if (!optionsWithoutConverter.Converters.Remove(this))
            Logger.LogWarning("cre8magic# Could not remove converter {Converter} from options", this);
        else
            Logger.LogInformation("cre8magic# Removed converter {Converter} from options", this);
        return optionsWithoutConverter;
    }

    protected T? ConvertObject(JsonObject jsonObject, JsonSerializerOptions options)
    {
        try
        {
            Logger.LogInformation("cre8magic# Deserializing {Type} from {Json}", typeof(T), jsonObject);

            IsInsideConverter = true;
            var result = jsonObject.Deserialize<T>(GetOptionsWithoutThisConverter(options));
            IsInsideConverter = false;

            return result;
        }
        catch
        {
            Logger.LogError("cre8magic# Error while deserializing {Type} from {Json}", typeof(T), jsonObject);
            IsInsideConverter = false; // Ensure the flag is reset even if an exception occurs
            throw;
        }
    }
}