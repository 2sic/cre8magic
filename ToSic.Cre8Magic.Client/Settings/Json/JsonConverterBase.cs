using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Settings.Json;

public abstract class JsonConverterBase<T>: JsonConverter<T>
{

    protected JsonSerializerOptions GetOptionsWithoutThisConverter(JsonSerializerOptions options)
    {
        JsonSerializerOptions optionsWithoutConverter = new(options);
        optionsWithoutConverter.Converters.Remove(this);
        return optionsWithoutConverter;
    }
    
    protected T? ConvertObject(JsonObject jsonObject, JsonSerializerOptions options)
        => jsonObject.Deserialize<T>(GetOptionsWithoutThisConverter(options));

}