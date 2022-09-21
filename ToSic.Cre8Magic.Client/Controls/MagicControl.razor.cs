using Microsoft.AspNetCore.Components;
using Oqtane.Themes;

namespace ToSic.Cre8Magic.Client.Controls;

/// <summary>
/// Oqtane Blazor Control with Settings
/// </summary>
public abstract class MagicControl: ThemeControlBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    protected bool UserIsAdmin => PageState.UserIsAdmin();

    protected bool UserIsEditor => PageState.UserIsEditor();

    protected bool UserIsLoggedIn => PageState.UserIsRegistered();

    public string? Classes(string target) => Settings.ThemeDesigner.Classes(target);
}