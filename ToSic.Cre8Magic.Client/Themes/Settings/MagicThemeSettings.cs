using static ToSic.Cre8Magic.Client.MagicConstants;

namespace ToSic.Cre8Magic.Client.Themes.Settings;

public class MagicThemeSettings: SettingsWithInherit, IHasDebugSettings
{
    /// <summary>
    /// The logo to show, should be located in the assets subfolder
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// The languages configuration which should be used
    /// </summary>
    public string? Languages { get; set; }

    public bool? LanguagesShow { get; set; } // = true;

    public int LanguagesMin { get; set; } = 0;

    // TODO:
    // - probably add properties like BreadcrumbShow
    // - consider how to model it - should we have sub-objects? or too complex?
    public bool? BreadcrumbsShow { get; set; } // = true;

    public bool? MagicContextInBody { get; set; }

    public string? Design { get; set; }

    /// <summary>
    /// Map of menu names and alternate configurations to load instead
    /// </summary>
    public NamedSettings<string> Menus { get; set; } = new();

    /// <summary>
    /// Name of the breadcrumbs configuration to use.
    /// Will usually be empty, as we'll use the Default instead
    /// </summary>
    public string? Breadcrumbs { get; set; }

    internal MagicThemeSettings Parse(ITokenReplace tokens)
    {
        Logo = tokens.Parse(Logo);
        return this;
    }

    public static MagicThemeSettings Fallback = new()
    {
        Logo = "unknown-logo.png",
        Languages = InheritName,
        LanguagesShow = true,
        LanguagesMin = 2,
        BreadcrumbsShow = true,
        MagicContextInBody = false,
        Breadcrumbs = InheritName,
        // The menus-map. Since this is the fallback, it must have at least an entry to not be skipped. 
        Menus = new()
        {
            { Default, Default }
        },
        Design = InheritName,
    };

    internal static Defaults<MagicThemeSettings> Defaults = new()
    {
        Fallback = Fallback,
        Foundation = Fallback
    };

    public MagicDebugSettings? Debug { get; set; }
}