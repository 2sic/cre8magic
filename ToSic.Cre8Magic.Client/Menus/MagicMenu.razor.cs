using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Menus;

/// <summary>
/// Base class for any menu list
/// </summary>
public abstract class MagicMenu : MagicMenuBase
{
#pragma warning disable CS8618
    [Parameter, EditorRequired] public MagicMenuBranch CurrentBranch { get; set; }
#pragma warning restore CS8618

    public string GetUrl(MagicMenuBranch branch) => GetUrl(branch.Page);

}