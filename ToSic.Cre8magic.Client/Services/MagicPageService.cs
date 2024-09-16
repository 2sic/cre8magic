using Oqtane.Security;
using Oqtane.Shared;
using Oqtane.UI;
using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client.Services
{
    /// <summary>
    /// This class provides functionality for the menu control.
    /// It is based on the core 'oqtane.framework\Oqtane.Client\Themes\Controls\Theme\MenuBase.cs'
    /// but it favors composition over inheritance.
    /// </summary>
    /// <remarks>
    /// Can't provide PageState from DI because that breaks Oqtane.
    /// </remarks>
    internal class MagicPageService(PageState pageState)
    {
        public IEnumerable<MagicPage> MenuPages => GetMenuPages();

        public string GetTarget(MagicPage page) 
            => page.Url?.StartsWith("http") == true ? "_new" : string.Empty;

        public string GetUrl(MagicPage page) 
            => (page.IsClickable) 
                ? string.IsNullOrEmpty(page.Url) ? NavigateUrl(page.Path) : page.Url
                : "javascript:void(0)";

        private IEnumerable<MagicPage> GetMenuPages()
        {
            if (pageState == null)
                throw new InvalidOperationException("PageState is null.");

            var securityLevel = int.MaxValue;
            foreach (var page in pageState.Pages.Where(item => item.IsNavigation))
            {
                if (page.Level <= securityLevel && UserSecurity.IsAuthorized(pageState.User, PermissionNames.View, page.PermissionList))
                {
                    securityLevel = int.MaxValue;
                    yield return page.ToMagicPage(pageState);
                }
                else if (securityLevel == int.MaxValue)
                {
                    securityLevel = page.Level;
                }
            }
        }

        private string NavigateUrl(string path, string parameters = "") 
            => Utilities.NavigateUrl(pageState.Alias.Path, path, parameters);
    }
}
