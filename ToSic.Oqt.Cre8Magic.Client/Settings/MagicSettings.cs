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
        //MagicBreadcrumbSettings breadcrumbs, 
        //MagicThemeDesignSettings themeDesign,
        //MagicContainerSettings container,
        //MagicContainerDesignSettings containerDesign, 
        TokenEngine tokens, 
        PageState pageState)
    {
        Layout = layout;
        //Breadcrumbs = breadcrumbs;
        //ThemeDesign = themeDesign;
        //Container = container;
        //ContainerDesign = containerDesign;
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

    public MagicBreadcrumbSettings Breadcrumbs => _b ??= Service.Breadcrumbs.Find(Layout.Breadcrumbs ?? Name, Name);
    private MagicBreadcrumbSettings? _b;

    public MagicThemeDesignSettings ThemeDesign => _td ??= Service.ThemeDesign.Find(Layout.PageDesign ?? Name, Name);
    private MagicThemeDesignSettings? _td;

    public MagicLanguagesSettings Languages => _l ??= Service.Languages.Find(Layout.Languages ?? Name, Name);
    private MagicLanguagesSettings? _l;

    public MagicLanguageDesignSettings LanguageDesign => _ld ??= Service.LanguageDesign.Find(Layout.LanguageMenuDesign ?? Name, Name);
    private MagicLanguageDesignSettings? _ld;

    public MagicContainerSettings Container => _c ??= Service.Containers.Find(Layout.Container ?? Name, Name);
    private MagicContainerSettings? _c;

    public MagicContainerDesignSettings ContainerDesign => _cd ??= Service.ContainerDesign.Find(Layout.ContainerDesign ?? Name, Name);
    private MagicContainerDesignSettings? _cd;

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;
}