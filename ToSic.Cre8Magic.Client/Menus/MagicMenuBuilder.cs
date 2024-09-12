using Microsoft.Extensions.Logging;
using ToSic.Cre8magic.Client.Models;
using ToSic.Cre8magic.Client.Settings.Json;

namespace ToSic.Cre8magic.Client.Menus;

/// <summary>
/// Will create a MenuTree based on the current pages information and configuration
/// </summary>
public class MagicMenuBuilder(MagicMenuTree magicMenuTree, ILogger<MagicMenuBuilder> logger) : MagicServiceWithSettingsBase
{

    public ILogger Logger { get; } = logger;

    private const string MenuSettingPrefix = "menu-";

    public MagicMenuTree GetTree(MagicMenuSettings config, List<MagicPage> menuPages)
    {
        var settingsSvc = Settings!.Service;
        var messages = new List<string>();
        var (configName, configMessages) = settingsSvc.FindConfigName(config.ConfigName, Settings.Name);
        messages.AddRange(configMessages);

        // Check if we have a name-remap to consider
        var menuConfig = Settings.ConfigurationName(configName);
        if (menuConfig == null && !configName.StartsWith(MenuSettingPrefix))
            menuConfig = Settings.ConfigurationName($"{MenuSettingPrefix}{configName}");

        var updatedName = menuConfig; // Settings.Theme.Menus.FindInvariant(configName);
        if (updatedName.HasValue())
        {
            configName = updatedName!;
            messages.Add($"updated config to '{configName}'");
        }

        // If the user didn't specify a config name in the Parameters or the config name
        // isn't contained in the json file the normal parameter are given to the service
        var menuSettings = settingsSvc.MenuSettings.Find(configName);
        config = JsonMerger.Merge(config, menuSettings, Logger);

        // See if we have a default configuration for CSS which should be applied
        var menuDesign = Settings.DesignName(configName);
        if (menuDesign == null && !configName.StartsWith(MenuSettingPrefix))
            menuDesign = Settings.DesignName($"{MenuSettingPrefix}{configName}");

        var designName = menuDesign;
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

        return magicMenuTree.Setup(Settings, config, menuPages, messages);
    }
}
