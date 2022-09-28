
namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// This is a catalog of all kinds of configurations.
/// It serves as a kind of database to manage all configurations, which will usually be retrieved using a name. 
/// </summary>
public class MagicSettingsCatalog: IHasDebugSettings
{
    public const string SourceDefault = "Unknown";
    /// <summary>
    /// Version number when loading from JSON to verify it's what we expect
    /// </summary>
    public float Version { get; set; }

    /// <summary>
    /// Master debug settings - would override other debugs
    /// </summary>
    public MagicDebugSettings? Debug { get; set; } = new();

    /// <summary>
    /// Source of these settings / where they came from, to ensure that we can see in debug where a value was picked up from
    /// </summary>
    public string Source { get; set; } = SourceDefault;

    public NamedSettings<MagicThemeSettings> Themes { get; set; } = new();
    public NamedSettings<MagicThemeDesignSettings> ThemeDesigns { get; set; } = new();

    public NamedSettings<MagicContainerSettings> Containers { get; set; } = new();

    public NamedSettings<MagicLanguagesSettings> Languages { get; set; } = new();

    /// <summary>
    /// The menu definitions
    /// </summary>
    public NamedSettings<MagicMenuSettings> Menus { get; set; } = new();

    /// <summary>
    /// Design definitions of the menu
    /// </summary>
    public NamedSettings<NamedSettings<MagicMenuDesign>> MenuDesigns { get; set; } = new();
}