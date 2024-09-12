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
    /// Use Init(PageState).
    /// </remarks>
    public class MagicPageService(/*PageState pageState*/)
    {
        private PageState? _pageState;

        #region Init

        /// <summary>
        /// PageState from constructor is moved here to Init method.
        /// </summary>
        /// <param name="pageState"></param>
        /// <returns></returns>
        public MagicPageService Init(PageState pageState)
        {
            _pageState = pageState;
            return this;
        } 
        #endregion

        public IEnumerable<MagicPage> MenuPages => GetMenuPages();

        public string GetTarget(MagicPage page) 
            => page.Url?.StartsWith("http") == true ? "_new" : string.Empty;

        public string GetUrl(MagicPage page) 
            => (page.IsClickable) 
                ? string.IsNullOrEmpty(page.Url) ? NavigateUrl(page.Path) : page.Url
                : "javascript:void(0)";

        private IEnumerable<MagicPage> GetMenuPages()
        {
            if (_pageState == null)
                throw new InvalidOperationException("PageState is null. Probably missing initialization with Init(PageState).");

            var securityLevel = int.MaxValue;
            foreach (var page in _pageState.Pages.Where(item => item.IsNavigation))
            {
                if (page.Level <= securityLevel && UserSecurity.IsAuthorized(_pageState.User, PermissionNames.View, page.PermissionList))
                {
                    securityLevel = int.MaxValue;
                    yield return page.ToMagicPage(this);
                }
                else if (securityLevel == int.MaxValue)
                {
                    securityLevel = page.Level;
                }
            }
        }

        private string NavigateUrl(string path, string parameters = "") 
            => Utilities.NavigateUrl(_pageState.Alias.Path, path, parameters);
    }
}
