using Microsoft.AspNetCore.Components;

namespace ToSic.Oqt.Cre8Magic.Client.Menus;

public abstract class MagicMenuBase: Oqtane.Themes.Controls.MenuBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

}