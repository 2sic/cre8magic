using Oqtane.Models;
using Oqtane.UI;

namespace ToSic.Cre8magic.Client.Models
{
    /// <summary>
    /// Wrapper for the Oqtane Page.
    /// </summary>
    /// <param name="originalPage"></param>
    /// <param name="pageState"></param>
    public class MagicPage(Page originalPage, PageState pageState)
    {
        /// <summary>
        /// Original Oqtane page wrapped in MagicPage.
        /// </summary>
        public Page OriginalPage { get; } = originalPage;

        /// <summary>
        /// Id of the Page
        /// </summary>
        public int PageId => OriginalPage.PageId;

        /// <summary>
        /// Reference to the parent <see cref="Page"/> if it has one.
        /// </summary>
        public int? ParentId => OriginalPage.ParentId;

        /// <summary>
        /// Path of the page.
        /// </summary>
        public string Path => OriginalPage.Path;

        /// <summary>
        /// Page Name.
        /// </summary>
        public string Name => OriginalPage.Name;

        /// <summary>
        /// Full URL to this page.
        /// </summary>
        public string Url => OriginalPage.Url;

        /// <summary>
        /// Link in site navigation is enabled or disabled.
        /// </summary>
        public bool IsClickable => OriginalPage.IsClickable;

        /// <summary>
        /// Current page level from the top of the Menu
        /// </summary>
        public int Level => OriginalPage.Level;

        /// <summary>
        /// Determines if there are sub-pages. True if this page has sub-pages.
        /// </summary>
        public bool HasChildren => OriginalPage.HasChildren;

        /// <summary>
        /// PageState id dependency that provides information about the current page,
        /// also it is used by derived classes MagicMenuPage, MagicMenuThree
        /// </summary>
        protected PageState PageState => pageState ?? throw new InvalidOperationException("PageState is null.");

        /// <summary>
        /// This service provides functionality for the menu control.
        /// It is based on the core 'oqtane.framework\Oqtane.Client\Themes\Controls\Theme\MenuBase.cs'
        /// but it favors composition over inheritance.
        /// </summary>
        private readonly MagicPageService _magicPageService = new(pageState);

        /// <summary>
        /// Link to this page.
        /// </summary>
        public string Link => _magicPageService.GetUrl(this);

        /// <summary>
        /// Target for link to this page.
        /// </summary>
        public string Target => _magicPageService.GetTarget(this);

        /// <summary>
        /// Menu pages for the current user.
        /// </summary>
        public IEnumerable<MagicPage> MenuPages => _magicPageService.MenuPages;
    }
}
