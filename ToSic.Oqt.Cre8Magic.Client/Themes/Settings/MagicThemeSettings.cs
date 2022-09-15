using static ToSic.Oqt.Cre8Magic.Client.MagicConstants;

namespace ToSic.Oqt.Cre8Magic.Client.Themes.Settings;

public class MagicThemeSettings
{
    /// <summary>
    /// The logo to show, should be located in the assets subfolder
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// The languages configuration which should be used
    /// </summary>
    public string? Languages { get; set; }

    // TODO:
    // - probably add properties like BreadcrumbShow
    // - consider how to model it - should we have sub-objects? or too complex?

    public bool LanguageMenuShow { get; set; } = true;

    public int LanguageMenuShowMin { get; set; } = 0;

    public string? LanguageMenuDesign { get; set; }

    public string? Container { get; set; }

    /// <summary>
    /// The preferred container design to use. 
    /// </summary>
    public string? ContainerDesign { get; set; }

    public bool? MagicContextInBody { get; set; }

    public string? PageDesign { get; set; }

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

    public static MagicThemeSettings Defaults = new()
    {
        Logo = "unknown-logo.png",
        Container = Inherit,
        ContainerDesign = Inherit,
        Languages = Inherit,
        LanguageMenuDesign = Inherit,
        LanguageMenuShow = true,
        LanguageMenuShowMin = 2,
        MagicContextInBody = false,
        Breadcrumbs = Inherit,
        // The menus-map. Since this is the fallback, it must have at least an entry to not be skipped. 
        Menus = new()
        {
            { Default, Default }
        },
        PageDesign = Inherit,
    };
}