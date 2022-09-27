using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Menus;

/// <summary>
/// Base class for Razor menus
/// </summary>
public abstract class MagicMenuRoot: MagicMenuBase
{
    /// <summary>
    /// Complex object with all settings.
    /// If this is used, all other settings will be ignored.
    /// </summary>
    [Parameter] public MagicMenuSettings? MenuSettings { get; set; }

    [Parameter] public string? MenuId { get; set; }
    [Parameter] public string? ConfigName { get; set; }
    ///// <inheritdoc />
    //[Parameter] public List<int>? PageList { get; set; }
    [Parameter] public bool? Children { get; set; }
    [Parameter] public bool? Debug { get; set; }
    [Parameter] public int? Depth { get; set; }
    [Parameter] public bool? Display { get; set; } = true;
    [Parameter] public int? Level { get; set; }
    [Parameter] public string? Start { get; set; }
    [Parameter] public string? Design { get; set; }

    [Parameter] public string? Template { get; set; }

    protected MagicMenuTree? Menu { get; private set; }

    protected MagicMenuBuilder? MenuTreeService { get; set; } = new();

    /// <summary>
    /// Detect if the menu is configured for vertical.
    /// For the most common 2 kinds of menu options. 
    /// </summary>
    protected bool IsVertical => MagicConstants.MenuVertical.EqInvariant(Menu?.Settings.Template);
    protected bool IsHorizontal => MagicConstants.MenuHorizontal.EqInvariant(Menu?.Settings.Template);

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        MenuTreeService!.InitSettings(Settings);

        var menuSettings = MenuSettings ?? new MagicMenuSettings
        {
            Id = MenuId,
            Children = Children,
            ConfigName = ConfigName,
            Debug = Debug == null ? null : new() { Allowed = Debug, Admin = Debug, Anonymous = Debug },
            Depth = Depth,
            Design = Design,
            Display = Display,
            Level = Level,
            Start = Start,
            Template = Template,
        };

        Menu = MenuTreeService?.GetTree(menuSettings, MenuPages.ToList());
    }

}