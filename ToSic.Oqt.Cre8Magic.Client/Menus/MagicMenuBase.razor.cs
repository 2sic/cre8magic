using Microsoft.AspNetCore.Components;
using ToSic.Oqt.Cre8Magic.Client.Controls;

namespace ToSic.Oqt.Cre8Magic.Client.Menus;

public abstract class MagicMenuBase: Oqtane.Themes.Controls.MenuBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

}