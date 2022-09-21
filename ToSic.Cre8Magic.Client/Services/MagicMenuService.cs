using Oqtane.Models;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Services;

/// <summary>
/// Will create a MenuTree based on the current pages information and configuration
/// </summary>
// TODO: MAYBE NOT A SERVICE - DOESN'T NEED DI atm
public class MagicMenuService: MagicServiceWithSettingsBase
{
    public MagicMenuTree GetTree(MagicMenuSettings config, List<Page> menuPages)
    {
        var settingsSvc = Settings!.Service;
        var (configName, debugInfo) = settingsSvc.FindConfigName(config.ConfigName, Settings.Name);

        // Check if we have a name-remap to consider
        var updatedName = Settings.Theme.Menus.FindInvariant(configName);
        if (updatedName.HasValue())
        {
            configName = updatedName!;
            debugInfo += $"; updated config to '{configName}'";
        }

        // If the user didn't specify a config name in the Parameters or the config name
        // isn't contained in the json file the normal parameter are given to the service
        var menuSettings = settingsSvc.MenuSettings.Find(configName);
        config = JsonMerger.Merge(config, menuSettings);

        // See if we have a default configuration for CSS which should be applied
        var designName = config.Design;
        debugInfo += $"; Design: '{designName}'";
        if (string.IsNullOrWhiteSpace(designName))
        {
            designName = configName;
            debugInfo += $"; Design changed to '{designName}'";
        }

        // Usually there is no Design-object pre-filled, in which case we should
        // 1. try to find it in json
        // 2. use the one from the configuration
        if (config.DesignSettings == null)
        {
            // Check various places where design could be configured by priority
            var designConfig = settingsSvc.MenuDesigns.Find(designName, Settings.Name);

            config.DesignSettings = designConfig; // = JsonMerger.Merge(new(config) { DesignSettings = designConfig }, config);
        }
        else
            debugInfo += "; Design rules already set";

        // should be null if not admin, so the final razor doesn't even add the attribute
        debugInfo = Settings.PageState.UserIsAdmin() ? debugInfo : null;

        return new(Settings, config, menuPages, debugInfo, settingsSvc);
    }
}