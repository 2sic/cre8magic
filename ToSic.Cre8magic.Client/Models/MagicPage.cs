using Oqtane.Models;

namespace ToSic.Cre8magic.Client.Models
{
    public class MagicPage(Page originalPage)
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
        /// Reference to the <see cref="Site"/>.
        /// </summary>
        public int SiteId => OriginalPage.SiteId;

        /// <summary>
        /// Path of the page.
        /// </summary>
        public string Path => OriginalPage.Path;

        /// <summary>
        /// Reference to the parent <see cref="Page"/> if it has one.
        /// </summary>
        public int? ParentId => OriginalPage.ParentId;

        /// <summary>
        /// Page Name.
        /// </summary>
        public string Name => OriginalPage.Name;

        /// <summary>
        /// Page Title which is shown in the browser tab.
        /// </summary>
        public string Title => OriginalPage.Title;

        /// <summary>
        /// Sort order in the list of other sibling pages
        /// </summary>
        public int Order => OriginalPage.Order;

        /// <summary>
        /// Full URL to this page.
        /// </summary>
        public string Url => OriginalPage.Url;

        /// <summary>
        /// Icon file for this page.
        /// </summary>
        public string Icon => OriginalPage.Icon;
        
        public bool IsNavigation => OriginalPage.IsNavigation;

        public bool IsClickable => OriginalPage.IsClickable;

        public bool IsPersonalizable => OriginalPage.IsPersonalizable;

        public int Level => OriginalPage.Level;

        public bool HasChildren => OriginalPage.HasChildren;
    }
}
