using static ToSic.Cre8Magic.Client.Themes.Settings.MagicThemeDesignSettings;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// As of now, doesn't support @inherits
/// </remarks>
public class MagicContainerDesignSettings : NamedSettings<MagicContainerDesignSettingsItem>
{
    //private const string IdKey = "Id";
    private const string IdDefault = "module-[Module.Id]";

    internal static Defaults<MagicContainerDesignSettings> Defaults = new()
    {
        Fallback = new()
        {
            {
                "div", new()
                {
                    Classes = $"{MainPrefix}-page-language {SettingFromDefaults}",
                    IsPublished = new(null, $"{ContainerDesigner.ModulePrefixDefault}-unpublished  {SettingFromDefaults}"),
                    IsAdminModule = new($"{MainPrefix}-admin-container  {SettingFromDefaults}"),
                    Id = IdDefault,
                }
            },
        },
        Foundation = new()
        {
            {
                "div", new()
                {
                    Id = IdDefault,
                }
            }
        }
    };
}