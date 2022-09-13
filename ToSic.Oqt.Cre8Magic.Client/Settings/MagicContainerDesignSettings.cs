using Oqtane.Models;
using ToSic.Oqt.Cre8Magic.Client.Styling;
using static ToSic.Oqt.Cre8Magic.Client.Styling.MagicPageDesign;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public class MagicContainerDesignSettings : NamedSettings<MagicContainerDesign>
{
    internal string? Classes(Module module, string tag)
    {
        var styles = this.FindInvariant(tag); // safe, also does null-check
        if (styles is null) return null;
        return string.Join(" ", new[]
        {
            module.IsPublished() ? styles.IsPublished : styles.IsNotPublished, // Info-Class if not published
            module.UseAdminContainer ? styles.IsAdminModule : styles.IsNotAdminModule // Info-class if admin module
        }.Where(s => s.HasValue()));

    }

    public static MagicContainerDesignSettings Defaults = new()
    {
        {
            "div", new()
            {
                Classes = $"{MainPrefix}page-language {SettingFromDefaults}",
                IsNotPublished = $"{ModulePrefixDefault}unpublished  {SettingFromDefaults}",
                IsAdminModule = $"{MainPrefix}admin-container  {SettingFromDefaults}"
            }
        },
    };
}