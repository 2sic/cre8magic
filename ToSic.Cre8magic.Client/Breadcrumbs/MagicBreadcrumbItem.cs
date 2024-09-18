using Oqtane.UI;
using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client.Breadcrumbs
{
    public class MagicBreadcrumbItem : MagicPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MagicBreadcrumbItem"/> class.
        /// </summary>
        /// <param name="pageState">The page state.</param>
        /// <param name="page">The original page.</param>
        public MagicBreadcrumbItem(PageState pageState, MagicPage? page = null, MagicBreadcrumb? breadcrumb = null) : base(page?.OriginalPage ?? pageState.Page)
        {
            PageState = pageState;
            MagicPageService = new MagicPageService(pageState);
            Breadcrumb = breadcrumb;

            if (page != null && breadcrumb != null)
                IsActive = (page.PageId == breadcrumb.PageId);
            else
                IsActive = true;
        }

        /// <summary>
        /// This service provides functionality for the menu control.
        /// It is based on the core 'oqtane.framework\Oqtane.Client\Themes\Controls\Theme\MenuBase.cs'
        /// but it favors composition over inheritance.
        /// </summary>
        private protected readonly MagicPageService MagicPageService;

        /// <summary>
        /// PageState id dependency that provides information about the current page,
        /// also it is used by derived classes MagicMenuPage, MagicMenuThree
        /// </summary>
        protected PageState PageState { get; }

        /// <summary>
        /// Root navigator object which has some data/logs for all navigators which spawned from it. 
        /// </summary>
        internal virtual MagicBreadcrumb Breadcrumb { get; }

        /// <summary>
        /// Determine if the breadcrumb item is current page.
        /// </summary>
        public bool IsActive { get; } = false;

        private ITokenReplace NodeReplace => _nodeReplace ??= Breadcrumb.PageTokenEngine(this);
        private ITokenReplace? _nodeReplace;

        /// <summary>
        /// Get css class for tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string? Classes(string tag) => NodeReplace.Parse(Breadcrumb.Design.Classes(tag, this)).EmptyAsNull();

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string? Value(string key) => NodeReplace.Parse(Breadcrumb.Design.Value(key, this)).EmptyAsNull();

        /// <summary>
        /// Link to this page.
        /// </summary>
        public string Link => MagicPageService.GetUrl(this);

        /// <summary>
        /// Target for link to this page.
        /// </summary>
        public string Target => MagicPageService.GetTarget(this);
    }
}
