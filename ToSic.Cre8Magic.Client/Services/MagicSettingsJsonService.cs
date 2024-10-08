﻿using Microsoft.Extensions.Logging;
using System.Text.Json;
using ToSic.Cre8magic.Client.Settings.Json;

namespace ToSic.Cre8magic.Client.Services;

public class MagicSettingsJsonService : IHasSettingsExceptions
{
    public ILogger<MagicSettingsJsonService> Logger { get; }

    public MagicSettingsJsonService(ILogger<MagicSettingsJsonService> logger)
    {
        Logger = logger;
    }
    
    public MagicSettingsCatalog LoadJson(MagicPackageSettings themeConfig)
    {
        var jsonFileName = $"{themeConfig.WwwRoot}/{themeConfig.Url}/{themeConfig.SettingsJsonFile}";
        try
        {
            var jsonString = File.ReadAllText(jsonFileName);
                
            var result = JsonSerializer.Deserialize<MagicSettingsCatalog>(jsonString, new JsonSerializerOptions(JsonMerger.GetNewOptionsForPreMerge(Logger))
            {
                PropertyNameCaseInsensitive = true,
                //ReadCommentHandling = JsonCommentHandling.Skip,
                //AllowTrailingCommas = true,
            })!;

            // Ensure we have version set, ATM exactly 0.01
            if (Math.Abs(result.Version - 0.01) > 0.001)
                AddException(themeConfig,
                    new ArgumentException($"Json {nameof(result.Version)} must be set to 0.01", nameof(result.Version)));

            if (!result.Source.HasValue() || result.Source == MagicSettingsCatalog.SourceDefault) 
                result.Source = "JSON";

            return result;
        }
        catch (Exception ex)
        {
            AddException(themeConfig, ex);
            return new();
        }
    }

    public List<Exception> Exceptions => _exceptions ??= new();
    private List<Exception>? _exceptions;

    private void AddException(MagicPackageSettings themeConfig, Exception ex)
    {
        Exceptions.Add(new SettingsException($"Error loading json configuration file '{themeConfig.SettingsJsonFile}'. {ex.Message}"));
    }
}