using Oqtane.Models;
using static ToSic.Oqt.Cre8Magic.Client.Themes.Settings.MagicThemeDesignSettings;

namespace ToSic.Oqt.Cre8Magic.Client.Containers.Settings;

public class MagicContainerDesignSettings : NamedSettings<MagicContainerDesignSettingsItem>
{
    private const string ModulePrefixDefault = "module";

    internal string? Classes(MagicSettings settings, Module module, string tag)
    {
        var styles = this.FindInvariant(tag); // safe, also does null-check
        if (styles is null) return null;

        var value = new ContainerDesigner(module).GetClasses(styles);
        var tokens = settings.Tokens.Expanded(new ModuleTokens(module));
        return tokens.Parse(value);
    }

    public static MagicContainerDesignSettings Defaults = new()
    {
        {
            "div", new()
            {
                Classes = $"{MainPrefix}-page-language {SettingFromDefaults}",
                IsNotPublished = $"{ModulePrefixDefault}-unpublished  {SettingFromDefaults}",
                IsAdminModule = $"{MainPrefix}-admin-container  {SettingFromDefaults}"
            }
        },
    };
}