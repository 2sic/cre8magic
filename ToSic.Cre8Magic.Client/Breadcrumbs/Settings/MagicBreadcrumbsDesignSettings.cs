using static ToSic.Cre8Magic.Client.Themes.Settings.MagicThemeDesignSettings;

namespace ToSic.Cre8Magic.Client.Breadcrumbs.Settings;

public class MagicBreadcrumbsDesignSettings : NamedSettings<DesignSettingActive>
{
    //internal string Classes(string tag, MagicLanguage? lang = null)
    //{
    //    if (!tag.HasValue()) return "";
    //    if (!this.Any()) return "";
    //    var styles = this.FindInvariant(tag);
    //    if (styles is null) return "";
    //    return styles.Classes + " " + styles.IsActive.Get(lang?.IsActive);
    //}

    internal static Defaults<MagicBreadcrumbsDesignSettings> Defaults = new()
    {
        Fallback = new()
        {
            { "div", new() { Classes = $"{MainPrefix}-page-breadcrumb {SettingFromDefaults}" } },
            {
                "a-home", new()
                {
                    Classes =
                        $"{MainPrefix}-page-breadcrumb-link {MainPrefix}-page-breadcrumb-home " +
                        $"{SettingFromDefaults}"
                }
            },
            { "revealer", new() { Classes = $"{MainPrefix}-page-breadcrumb-trigger"}},
            { "a", new() { Classes = $"{MainPrefix}-page-breadcrumb-link" } },
            //{ "span", new() },
        },
    };
}