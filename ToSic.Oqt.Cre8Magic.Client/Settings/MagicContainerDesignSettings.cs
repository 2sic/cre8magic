using Oqtane.Models;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Styling;
using ToSic.Oqt.Cre8Magic.Client.Tokens;
using static ToSic.Oqt.Cre8Magic.Client.Styling.MagicPageDesign;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public class MagicContainerDesignSettings : NamedSettings<MagicContainerDesign>
{
    internal string? Classes(PageState pageState, Module module, string tag)
    {
        var styles = this.FindInvariant(tag); // safe, also does null-check
        if (styles is null) return null;

        var tokens = new ModuleTokens(pageState, module);
        return tokens.Replace(styles);
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