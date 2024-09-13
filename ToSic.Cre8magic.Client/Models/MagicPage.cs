using Oqtane.Models;

namespace ToSic.Cre8magic.Client.Models
{
    public class MagicPage(Page originalPage)
    {
        /// <summary>
        /// Connect to the MagicPageService to get additional information about the page.
        /// </summary>
        /// <param name="magicPageService"></param>
        /// <returns></returns>
        public MagicPage Init(MagicPageService magicPageService)
        {
            _magicPageService = magicPageService;
            return this;
        }
        private MagicPageService _magicPageService;

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

        public bool IsClickable => OriginalPage.IsClickable;

        public int Level => OriginalPage.Level;

        public bool HasChildren => OriginalPage.HasChildren;

        public string Link => _magicPageService.GetUrl(this);

        public string Target => _magicPageService.GetTarget(this);

        public IEnumerable<MagicPage> MenuPages => _magicPageService.MenuPages;
    }
}
