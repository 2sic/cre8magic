using Microsoft.AspNetCore.Components;
using Oqtane.Themes;

namespace ToSic.Cre8magic.Client.Controls;

/// <summary>
/// Oqtane Blazor Control with Settings
/// </summary>
public abstract class MagicControl: ThemeControlBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    protected bool UserIsAdmin => PageState.UserIsAdmin();

    protected bool UserIsEditor => PageState.UserIsEditor();

    protected bool UserIsLoggedIn => PageState.UserIsRegistered();

    protected virtual IMagicDesigner Designer => _designer ??= Settings?.ThemeDesigner;
    private IMagicDesigner? _designer;

    public string? Classes(string target) => Designer.Classes(target);

    public string? ClassesOrDefault(string target, string defaultValue) => Classes(target) ?? defaultValue;

    public string? Value(string target) => Designer.Value(target);

    public string? Id(string name) => Designer.Id(name);
}