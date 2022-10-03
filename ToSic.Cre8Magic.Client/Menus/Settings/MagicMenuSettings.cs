namespace ToSic.Cre8Magic.Client.Menus.Settings;

public class MagicMenuSettings : SettingsWithInherit, ICloneAndMerge<MagicMenuSettings>, IHasDebugSettings
{
    /// <summary>
    /// Empty constructor is important for JSON deserialization
    /// </summary>
    public MagicMenuSettings() { }

    /// <summary>
    /// A unique ID to identify the menu.
    /// Would be used for debugging but also to help in creating unique css-classes for collapsible menus
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name to identify this configuration
    /// </summary>
    // TODO: REVIEW NAME
    public string? ConfigName { get; set; }

    /// <inheritdoc />
    public MagicDebugSettings? Debug { get; set; }

    /// <summary>
    /// Determines if this navigation should be shown.
    /// Mainly used for standard menus which could be disabled through configuration. 
    /// </summary>
    // TODO: REVIEW NAME - Show would probably be better!
    public bool? Display { get; set; } = DisplayDefault;
    public const bool DisplayDefault = true;

    /// <summary>
    /// How many level deep the navigation should show.
    /// The number is ??? relative,
    /// so if the navigation starts an level 2 then levelDepth 2 means to show levels 2 & 3 ??? verify
    /// </summary>
    public int? Depth { get; set; }
    public const int LevelDepthFallback = 1;

    /// <summary>
    /// Levels to skip from the initial stating point.
    /// - 0 means don't skip any, so if we're starting at the root, show that level
    /// - 1 means skip the first level, so if we're starting at the root, show the children
    /// See inspiration context from DDRMenu https://www.dnnsoftware.com/wiki/ddrmenu-reference-guide
    /// in DDR it was called 'skip' but it didn't make sense IMHO
    /// </summary>
    public bool? Children { get; set; }
    public const bool ChildrenFallback = default;

    //// TODO: NOT YET IMPLEMENTED
    ///// <summary>
    ///// Exact list of pages to show in this menu.
    ///// TODO: MAYBE allow for negative numbers to remove them from the list?
    ///// </summary>
    //public List<int>? PageList { get; set; }

    /// <summary>
    /// Start page of this navigation - like home or another specific page.
    /// Can be
    /// - a specific ID
    /// - a CSV of IDs ???
    /// - `*` to indicate all pages on the specified level
    /// - `.` to indicate current page
    /// - blank / null, to use another start ???
    /// </summary>
    public string? Start { get; set; }
    public const string StartPageRoot = "*";
    public const string StartPageCurrent = ".";

    /// <summary>
    /// The level this menu should start from.
    /// - `0` is the top level (default)
    /// - `1` is the top level containing home and other pages
    /// - `-1` is one level up from the current node
    /// - `-2` is two levels up from the current node
    /// </summary>
    public int? Level { get; set; }
    public const int StartLevelFallback = default;

    /// <summary>
    /// The template to use - horizontal/vertical
    /// </summary>
    public string? Template { get; set; }
    public const string TemplateDefault = "Horizontal";

    // todo: name, maybe not on interface
    public NamedSettings<MagicMenuDesign>? DesignSettings { get; set; }

    public string MenuId => _menuId ??= Id.HasText()
        ? Id
        : new Random().Next(100000, 1000000).ToString();
    private string? _menuId;


    private static readonly MagicMenuSettings FbAndF = new()
    {
        Template = TemplateDefault,
        Start = "*",
        Depth = 0,
    };

    internal static Defaults<MagicMenuSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };
}