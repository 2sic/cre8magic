using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using Oqtane.UI;

namespace ToSic.Cre8Magic.Client;

public static class PageStateSecurityExtensions
{
    internal static bool UserIsEditor(this PageState pageState)
        => pageState.User != null && UserSecurity.IsAuthorized(pageState.User, PermissionNames.Edit, pageState.Page.Permissions);

    internal static bool UserIsAdmin(this PageState pageState)
        => pageState.User != null && UserSecurity.IsAuthorized(pageState.User, PermissionNames.Edit, pageState.Page.Permissions);

    internal static bool UserIsRegistered(this PageState pageState)
        => pageState.User != null && UserSecurity.IsAuthorized(pageState.User, RoleNames.Registered);

    /// <summary>
    /// Modules are treated as admin modules (and must use the the admin container) if they are marked as such, or come from the Oqtane ....Admin... type
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    internal static bool ForceAdminContainer(this Module module) 
        => module.UseAdminContainer || module.ModuleType.Contains(".Admin.");

    internal static bool IsPublished(this Module module)
        => UserSecurity.ContainsRole(module.Permissions, PermissionNames.View, RoleNames.Everyone);

}