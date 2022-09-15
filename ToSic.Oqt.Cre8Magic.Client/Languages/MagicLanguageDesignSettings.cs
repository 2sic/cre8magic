using static ToSic.Oqt.Cre8Magic.Client.Settings.Themes.MagicThemeDesignSettings;

namespace ToSic.Oqt.Cre8Magic.Client.Languages;

public class MagicLanguageDesignSettings: NamedSettings<DesignSettingActive>
{
    internal string Classes(string tag, MagicLanguage? lang = null)
    {
        if (!tag.HasValue()) return "";
        if (!this.Any()) return "";
        var styles = this.FindInvariant(tag);
        if (styles is null) return "";
        return styles.Classes + " " + (lang?.IsActive ?? false ? styles.IsActive : styles.IsNotActive);
    }

    public static MagicLanguageDesignSettings Defaults = new()
    {
        { "ul", new() { Classes = $"{MainPrefix}-page-language {SettingFromDefaults}" } },
        { "li", new() { IsActive = $"active {SettingFromDefaults}", IsNotActive = "" } }
    };
}