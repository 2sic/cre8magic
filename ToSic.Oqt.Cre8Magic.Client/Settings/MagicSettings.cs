using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;
using static System.StringComparer;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

/// <summary>
/// The current settings of a page.
/// </summary>
public class MagicSettings: IHasSettingsExceptions
{
    internal MagicSettings(
        string name,
        MagicSettingsService service,
        MagicThemeSettings layout, 
        MagicBreadcrumbSettings breadcrumb, 
        MagicThemeDesignSettings themeDesign, 
        MagicLanguagesSettings languages, 
        MagicLanguageDesignSettings languageDesign, 
        MagicContainerSettings container,
        MagicContainerDesignSettings containerDesign, 
        TokenEngine tokens, 
        PageState pageState)
    {
        Layout = layout;
        Breadcrumb = breadcrumb;
        ThemeDesign = themeDesign;
        Languages = languages;
        LanguageDesign = languageDesign;
        Container = container;
        ContainerDesign = containerDesign;
        Tokens = tokens;
        PageState = pageState;
        Name = name;
        Service = service;
    }

    public bool Debug => Service.Debug;

    internal PageState PageState { get; }

    internal TokenEngine Tokens { get; }

    public string MagicContext { get; set; } = "";

    public string Name { get; }

    public MagicSettingsService Service { get; }

    public MagicThemeSettings Layout { get; }

    public MagicBreadcrumbSettings Breadcrumb { get; }

    public MagicThemeDesignSettings ThemeDesign { get; }

    public MagicLanguagesSettings Languages { get; }

    public MagicLanguageDesignSettings LanguageDesign { get; set; }

    public MagicContainerSettings Container { get; set; }

    public MagicContainerDesignSettings ContainerDesign { get; set; }

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;
}