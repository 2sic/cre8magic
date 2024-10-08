﻿using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8magic.Client.Containers;

public class MagicContainer: Oqtane.Themes.ContainerBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    #region Navigation / Close

    [Inject] public NavigationManager? NavigationManager { get; set; }

    protected void CloseModal() => NavigationManager?.NavigateTo(NavigateUrl());
    
    #endregion
    
    private ContainerDesigner Designer => _designer ??= new(Settings, ModuleState);
    private ContainerDesigner? _designer;

    public string? Classes(string target) => Designer.Classes(target);
    public string? Id(string name) => Designer.Id(name);
    public string? Value(string key) => Designer.Value(key);

    /// <summary>
    /// Modules are treated as admin modules (and must use the the admin container) if they are marked as such, or come from the Oqtane ....Admin... type
    /// </summary>
    /// <returns></returns>
    protected bool UseAdminContainer => ModuleState.ForceAdminContainer();

}