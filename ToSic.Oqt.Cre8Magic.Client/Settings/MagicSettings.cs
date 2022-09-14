using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Styling;
using ToSic.Oqt.Cre8Magic.Client.Tokens;
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
        MagicLayoutSettings layout, 
        MagicBreadcrumbSettings breadcrumb, 
        MagicPageDesignSettings page, 
        MagicLanguagesSettings languages, 
        MagicLanguageDesignSettings languageDesign, 
        MagicContainerSettings container,
        MagicContainerDesignSettings containerDesign, 
        TokenEngine tokens, 
        PageState pageState)
    {
        Layout = layout;
        Breadcrumb = breadcrumb;
        Page = page;
        Languages = languages;
        LanguageDesign = languageDesign;
        Container = container;
        ContainerDesign = containerDesign;
        Tokens = tokens;
        PageState = pageState;
        Name = name;
        Service = service;
    }

    internal PageState PageState { get; }

    internal TokenEngine Tokens { get; }

    public string MagicContext { get; set; } = "";

    public string Name { get; }

    public MagicSettingsService Service { get; }

    public MagicLayoutSettings Layout { get; }

    public MagicBreadcrumbSettings Breadcrumb { get; }

    public MagicPageDesignSettings Page { get; }

    public MagicLanguagesSettings Languages { get; }

    public MagicLanguageDesignSettings LanguageDesign { get; set; }

    public MagicContainerSettings Container { get; set; }

    public MagicContainerDesignSettings ContainerDesign { get; set; }

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;
}