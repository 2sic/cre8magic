using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Menus;

public abstract class MagicMenuBase: Oqtane.Themes.Controls.MenuBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    public string? Classes(string target) => $"error calling {nameof(Classes)} in {nameof(MagicMenuBase)}. Use the Classes method of the branch to get the expected result.";
    public string? Value(string target) => $"error calling {nameof(Value)} in {nameof(MagicMenuBase)}. Use the Classes method of the branch to get the expected result.";
}