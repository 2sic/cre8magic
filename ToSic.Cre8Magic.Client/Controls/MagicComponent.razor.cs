using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Controls;

public abstract class MagicComponent: ComponentBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }
}