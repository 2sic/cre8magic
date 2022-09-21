using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Controls;

/// <summary>
/// Non-Oqtane Blazor component with Settings as base for your controls
/// </summary>
public abstract class MagicComponent: ComponentBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }
}