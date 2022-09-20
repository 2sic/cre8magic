﻿using System.Text.Json.Serialization;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;
using static System.StringComparer;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

/// <summary>
/// The current settings of a page.
/// </summary>
public class MagicSettings: IHasSettingsExceptions, IHasDebugSettings
{
    internal MagicSettings(string name, MagicSettingsService service, MagicThemeSettings theme, TokenEngine tokens, PageState pageState)
    {
        Name = name;
        Service = service;
        Theme = theme;
        Tokens = tokens;
        PageState = pageState;
    }

    public MagicDebugState Debug => _debug ??= DebugState(Theme); // Service.Debug.Parsed(PageState.UserIsAdmin());
    private MagicDebugState? _debug;

    /// <summary>
    /// This is only used to detect if debugging should be active, and the setting should come from the theme itself
    /// </summary>
    MagicDebugSettings? IHasDebugSettings.Debug => Theme.Debug;


    public MagicDebugState DebugState(object? target) => Service.Debug.GetState(target, PageState.UserIsAdmin());

    internal PageState PageState { get; }

    internal TokenEngine Tokens { get; }

    public string MagicContext { get; set; } = "";

    public string Name { get; }

    [JsonIgnore]
    public MagicSettingsService Service { get; }

    public MagicThemeSettings Theme { get; }

    public MagicBreadcrumbSettings Breadcrumbs => _b ??= Service.Breadcrumbs.Find(Theme.Breadcrumbs ?? Name, Name);
    private MagicBreadcrumbSettings? _b;

    public MagicThemeDesignSettings ThemeDesign => _td ??= Service.ThemeDesign.Find(Theme.PageDesign ?? Name, Name);
    private MagicThemeDesignSettings? _td;

    public MagicLanguagesSettings Languages => _l ??= Service.Languages.Find(Theme.Languages ?? Name, Name);
    private MagicLanguagesSettings? _l;

    public MagicLanguageDesignSettings LanguageDesign => _ld ??= Service.LanguageDesign.Find(Theme.LanguageMenuDesign ?? Name, Name);
    private MagicLanguageDesignSettings? _ld;

    public MagicContainerSettings Container => _c ??= Service.Containers.Find(Theme.Container ?? Name, Name);
    private MagicContainerSettings? _c;

    public MagicContainerDesignSettings ContainerDesign => _cd ??= Service.ContainerDesign.Find(Theme.ContainerDesign ?? Name, Name);
    private MagicContainerDesignSettings? _cd;

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;

}