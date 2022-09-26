using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Containers;

public class MagicContainer: Oqtane.Themes.ContainerBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    [Inject] public NavigationManager? NavigationManager { get; set; }

    public int ModuleId => ModuleState.ModuleId;

    protected void CloseModal() => NavigationManager?.NavigateTo(NavigateUrl());

    public string? Classes(string target) => Designer.Classes(target).EmptyAsNull();

    private ContainerDesigner Designer => _designer ??= new(Settings, ModuleState);
    private ContainerDesigner? _designer;

    public string? Value(string key) => Settings.Container.Value(Settings, ModuleState, key);

    public string? Id(string name) => Designer.Id(name);

    /// <summary>
    /// Modules are treated as admin modules (and must use the the admin container) if they are marked as such, or come from the Oqtane ....Admin... type
    /// </summary>
    /// <returns></returns>
    protected bool ForceAdminContainer => ModuleState.ForceAdminContainer();

}