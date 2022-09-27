using Microsoft.AspNetCore.Components;
using ToSic.Cre8Magic.Client.Settings.Json;

namespace ToSic.Cre8Magic.Client.Menus;

/// <summary>
/// Base class for Razor menus
/// </summary>
public abstract class MagicMenuRoot: MagicMenuBase
{
    // TODO: USE THIS INSTEAD
    [Parameter] public MagicMenuSettings? MenuSettings { get; set; }

    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }
    /// <inheritdoc />
    [Parameter] public string? ConfigName { get; set; }
    ///// <inheritdoc />
    //[Parameter] public List<int>? PageList { get; set; }
    ///// <inheritdoc />
    //[Parameter] public bool? Children { get; set; }
    ///// <inheritdoc />
    //[Parameter] public int? Depth { get; set; }
    ///// <inheritdoc />
    //[Parameter] public bool? Display { get; set; } = true;
    ///// <inheritdoc />
    //[Parameter] public int? Level { get; set; }
    /// <inheritdoc />
    [Parameter] public string? Start { get; set; }
    ///// <inheritdoc />
    //[Parameter] public string? Design { get; set; }

    //[Parameter] public string? Template { get; set; }

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

        var tempSettings = new MagicMenuSettings
        {
            Id = Id,
            ConfigName = ConfigName,
            Start = Start,
            
        };
        var combined = MenuSettings == null 
            ? tempSettings 
            : JsonMerger.Merge(tempSettings, MenuSettings);

        Menu = MenuTreeService?.GetTree(combined, MenuPages.ToList());
    }

}