using System.Text.Json.Serialization;
using Oqtane.UI;
using ToSic.Cre8Magic.Client.Analytics;
using static System.StringComparer;

namespace ToSic.Cre8Magic.Client.Settings;

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

    public MagicDebugState Debug => _debug ??= DebugState(Theme);
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

    [JsonIgnore] public MagicSettingsService Service { get; }
    [JsonIgnore] internal ThemeDesigner ThemeDesigner => _themeDesigner ??= new(this);
    private ThemeDesigner? _themeDesigner;

    public MagicThemeSettings Theme { get; }

    /// <summary>
    /// Determine if we should show a specific part
    /// </summary>
    public bool Show(string name) => Theme.Parts.TryGetValue(name, out var value) && value.Show == true;

    /// <summary>
    /// Determine the name of the design configuration of a specific part
    /// </summary>
    internal string? DesignName(string name) => Theme.Parts.TryGetValue(name, out var value) ? value.Design : null;

    /// <summary>
    /// Determine the configuration name of a specific part.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal string? ConfigurationName(string name) => Theme.Parts.TryGetValue(name, out var value) ? value.Configuration : null;

    internal string ConfigurationNameOrDefault(string name) => ConfigurationName(name) ?? Name;

    public MagicAnalyticsSettings Analytics => _a ??= Service.Analytics.Find(ConfigurationNameOrDefault(nameof(Analytics)), Name);
    private MagicAnalyticsSettings? _a;

    public MagicThemeDesignSettings ThemeDesign => _td ??= Service.ThemeDesign.Find(Theme.Design ?? ConfigurationNameOrDefault(nameof(Theme.Design)), Name);
    private MagicThemeDesignSettings? _td;

    public MagicLanguagesSettings Languages => _l ??= Service.Languages.Find(ConfigurationNameOrDefault(nameof(Languages)), Name);
    private MagicLanguagesSettings? _l;

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;

}