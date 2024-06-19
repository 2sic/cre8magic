namespace ToSic.Cre8magic.Client.Themes.Settings;

public class MagicThemePartSettings
{
    /// <summary>
    /// For json
    /// </summary>
    public MagicThemePartSettings() {}

    public MagicThemePartSettings(bool show)
    {
        Show = show;
        Design = null;
        Configuration = null;
    }

    public MagicThemePartSettings(string name)
    {
        Show = true;
        Design = name;
        Configuration = name;
    }

    public bool? Show { get; set; }
    public string? Design { get; set; }
    public string? Configuration { get; set; }
}