namespace ToSic.Oqt.Cre8Magic.Client.Menus.Settings;

public class MagicMenuSettings : SettingsWithInherit, IMagicMenuSettings, ICloneAndMerge<MagicMenuSettings>
{

    /// <summary>
    /// Empty constructor is important for JSON deserialization
    /// </summary>
    public MagicMenuSettings() { }

    //public MagicMenuSettings(IMagicMenuSettings original)
    //{
    //    Id = original.Id;
    //    ConfigName = original.ConfigName;
    //    Debug = original.Debug;
    //    Display = original.Display;
    //    Depth = original.Depth;
    //    Children = original.Children;
    //    PageList = original.PageList;
    //    Start = original.Start;
    //    Level = original.Level;
    //    Template = original.Template;

    //    Design = original.Design;
    //    DesignSettings = (original as MagicMenuSettings)?.DesignSettings;
    //}


    /// <inheritdoc />
    public string? Id { get; set; }

    /// <inheritdoc />
    public string? ConfigName { get; set; }

    /// <inheritdoc />
    public bool Debug { get; set; } = DebugDefault;

    public const bool DebugDefault = false;

    /// <inheritdoc />
    public bool? Display { get; set; } = DisplayDefault;
    public const bool DisplayDefault = true;

    /// <inheritdoc />
    public int? Depth { get; set; }
    public const int LevelDepthFallback = default;

    /// <inheritdoc />
    public bool? Children { get; set; }
    public const bool ChildrenFallback = default;

    /// <inheritdoc />
    public List<int>? PageList { get; set; }

    /// <inheritdoc />
    public string? Start { get; set; }
    public const string StartPageRoot = "*";
    public const string StartPageCurrent = ".";

    /// <inheritdoc />
    public int? Level { get; set; }
    public const int StartLevelFallback = default;

    public string? Design { get; set; }

    public string? Template { get; set; }
    public const string TemplateDefault = "Horizontal";

    // todo: name, maybe not on interface
    public MagicMenuDesignSettings? DesignSettings { get; set; }

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