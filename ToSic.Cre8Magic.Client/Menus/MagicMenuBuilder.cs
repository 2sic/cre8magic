using Oqtane.Models;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Services;

/// <summary>
/// Will create a MenuTree based on the current pages information and configuration
/// </summary>
public class MagicMenuBuilder: MagicServiceWithSettingsBase
{
    public MagicMenuTree GetTree(MagicMenuSettings config, List<Page> menuPages)
    {
        var settingsSvc = Settings!.Service;
        var messages = new List<string>();
        var (configName, configMessages) = settingsSvc.FindConfigName(config.ConfigName, Settings.Name);
        messages.AddRange(configMessages);

        // Check if we have a name-remap to consider
        var updatedName = Settings.Theme.Menus.FindInvariant(configName);
        if (updatedName.HasValue())
        {
            configName = updatedName!;
            messages.Add($"updated config to '{configName}'");
        }

        // If the user didn't specify a config name in the Parameters or the config name
        // isn't contained in the json file the normal parameter are given to the service
        var menuSettings = settingsSvc.MenuSettings.Find(configName);
        config = JsonMerger.Merge(config, menuSettings);

        // See if we have a default configuration for CSS which should be applied
        var designName = config.Design;
        messages.Add($"Design name in config: '{designName}'");
        if (string.IsNullOrWhiteSpace(designName))
        {
            designName = configName;
            messages.Add($"Design set to '{designName}'");
        }

        // Usually there is no Design-object pre-filled, in which case we should
        // 1. try to find it in json
        // 2. use the one from the configuration
        if (config.DesignSettings == null)
        {
            // Check various places where design could be configured by priority
            var designConfig = settingsSvc.MenuDesigns.Find(designName, Settings.Name);

            config.DesignSettings = designConfig;
        }
        else
            messages.Add("Design rules already set");

        return new(Settings, config, menuPages, messages);
    }
}