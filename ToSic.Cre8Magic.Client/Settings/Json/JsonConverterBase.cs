using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace ToSic.Cre8Magic.Client.Settings.Json;

public abstract class JsonConverterBase<T>: JsonConverter<T>
{
    public ILogger Logger { get; }

    protected JsonConverterBase(ILogger logger) => Logger = logger;


    protected JsonSerializerOptions GetOptionsWithoutThisConverter(JsonSerializerOptions options)
    {
        JsonSerializerOptions optionsWithoutConverter = new(options);
        optionsWithoutConverter.Converters.Remove(this);
        return optionsWithoutConverter;
    }
    
    protected T? ConvertObject(JsonObject jsonObject, JsonSerializerOptions options)
    {
        try
        {
            Logger.LogTrace("Deserializing {Type} from {Json}", typeof(T), jsonObject);
            return jsonObject.Deserialize<T>(GetOptionsWithoutThisConverter(options));
        }
        catch
        {
            Logger.LogError("Error while deserializing {Type} from {Json}", typeof(T), jsonObject);
            throw;
        }
    }
}