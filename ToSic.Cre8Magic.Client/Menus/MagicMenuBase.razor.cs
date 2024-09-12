using Microsoft.AspNetCore.Components;

// TODO: adapt Cre8magic everywhere
namespace ToSic.Cre8magic.Client.Menus;

public abstract class MagicMenuBase: Oqtane.Themes.Controls.MenuBase, IMagicControlWithSettings // TODO: stv use ThemeControlBase instead MenuBase and use MenuPageService that replaces MenuBase
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    private const string ErrMsg = "error calling {0} in {1}. Use the {0} method of the branch to get the expected result.";

    public string? Classes(string target) => string.Format(ErrMsg, nameof(Classes), nameof(MagicMenuBase));
    public string? Value(string target) => string.Format(ErrMsg, nameof(Value), nameof(MagicMenuBase));

    public string? Id(string target) => string.Format(ErrMsg, nameof(Id), nameof(MagicMenuBase));
}