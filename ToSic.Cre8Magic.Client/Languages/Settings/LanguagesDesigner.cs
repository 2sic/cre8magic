namespace ToSic.Cre8magic.Client.Languages.Settings;

internal class LanguagesDesigner: ThemeDesigner
{
    public LanguagesDesigner(MagicSettings settings): base(settings) { }

    internal string Classes(MagicLanguage? lang, string tag)
    {
        if (!tag.HasValue()) return "";
        var styles = GetSettings(tag);
        if (styles is null) return "";
        return styles.Classes + " " + styles.IsActive.Get(lang?.IsActive);
    }

}