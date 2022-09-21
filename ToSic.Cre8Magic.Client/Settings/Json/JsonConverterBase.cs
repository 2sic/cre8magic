using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Settings.Json;

public abstract class JsonConverterBase<T>: JsonConverter<T>
{

    protected JsonSerializerOptions GetOptionsWithoutThisConverter(JsonSerializerOptions options)
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

    protected T? ConvertObject(JsonObject jsonObject, JsonSerializerOptions options)
        => jsonObject.Deserialize<T>(GetOptionsWithoutThisConverter(options));

}