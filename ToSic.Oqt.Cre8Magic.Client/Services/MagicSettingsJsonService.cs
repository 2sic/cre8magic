using System.Text.Json;

namespace ToSic.Oqt.Cre8Magic.Client.Services;

public class MagicSettingsJsonService : IHasSettingsExceptions
{
    public MagicSettingsCatalog LoadJson(MagicPackageSettings themeConfig)
    {
        var jsonFileName = $"{themeConfig.WwwRoot}/{themeConfig.Url}/{themeConfig.SettingsJsonFile}";
        try
        {
            var jsonString = File.ReadAllText(jsonFileName);
                
            var result = JsonSerializer.Deserialize<MagicSettingsCatalog>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            })!;

            // Ensure we have version set
            if (Math.Abs(result.Version - 0.1) > 0.001) 
                throw new ArgumentException($"{nameof(result.Version)} must be set to 0.01", nameof(result.Version));

            if (!result.Source.HasValue()) result.Source = "JSON";

            return result;
        }
        catch (Exception ex)
        {
            Exceptions.Add(new($"Error loading json configuration file '{themeConfig.SettingsJsonFile}'. {ex.Message}"));
            //throw;//wip
            // probably no json file found?
            return new();
        }
    }

    public List<SettingsException> Exceptions => _exceptions ??= new();
    private List<SettingsException>? _exceptions;
}