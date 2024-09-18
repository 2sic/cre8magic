using Oqtane.UI;
using ToSic.Cre8magic.Client.Breadcrumbs.Settings;
using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client.Breadcrumbs
{
    public class MagicBreadcrumb : MagicBreadcrumbItem
    {
        private readonly MagicPage _homePage;

        public MagicBreadcrumb(PageState pageState) : base(pageState)
        {
            _homePage = pageState.Pages.First(p => p.Path == "").ToMagicPage();
            StartPage = PageState.Page.ToMagicPage();
            Settings = MagicBreadcrumbSettings.Defaults.Fallback;
            Design = new MagicBreadcrumbDesigner(this, Settings);
        }

        public MagicBreadcrumb(MagicSettings magicSettings) : this(magicSettings.PageState)
        {
            MagicSettings = magicSettings;
        }

        #region Init
        public MagicBreadcrumb Setup(MagicBreadcrumbSettings settings)
        {
            Settings = settings;
            StartPage = Settings.Start.HasValue
                ? PageState.Pages.FirstOrDefault(p => p.PageId == Settings.Start)?.ToMagicPage() ?? PageState.Page.ToMagicPage()
                : PageState.Page.ToMagicPage();
            return this;
        }

        public MagicBreadcrumb Designer(IBreadcrumbDesigner designer)
        {
            Design = designer;
            return this;
        }
        #endregion

        public MagicBreadcrumbSettings Settings { get; private set; }

        private MagicSettings? MagicSettings { get; }

        public MagicPage StartPage { get; private set; }

        public List<MagicBreadcrumbItem> Items => GetBreadcrumbs();
        //public List<MagicPage> Breadcrumbs => pageState.Breadcrumbs(_currentPage).ToList();

        internal IBreadcrumbDesigner Design { get; private set; }

        internal override MagicBreadcrumb Breadcrumb => this;

        internal TokenEngine? PageTokenEngine(MagicPage page)
        {
            // fallback without MagicSettings return just TokenEngine with PageTokens
            if (MagicSettings == null)
                return new TokenEngine([
                    new PageTokens(PageState, page)
                ]);

            var originalPage = (PageTokens)MagicSettings.Tokens.Parsers.First(p => p.NameId == PageTokens.NameIdConstant);
            originalPage = originalPage.Modified(page);
            return MagicSettings.Tokens.SwapParser(originalPage);
        }

        #region Private Methods

        private List<MagicBreadcrumbItem> GetBreadcrumbs(MagicPage? page = null)
        {
            var currentPage = page ?? StartPage;
            var breadcrumbs = new List<MagicBreadcrumbItem>();

            if (_homePage.PageId == currentPage.PageId) return breadcrumbs;

            breadcrumbs.Insert(0, new MagicBreadcrumbItem(PageState, _homePage));

            var parentPage = PageState.Pages.FirstOrDefault(p => p.PageId == currentPage.ParentId);
            while (parentPage != null && _homePage.PageId != parentPage.PageId)
            {
                breadcrumbs.Insert(1, new MagicBreadcrumbItem(PageState, parentPage!.ToMagicPage()));
                parentPage = PageState.Pages.FirstOrDefault(p => p.PageId == parentPage.ParentId);
            }
            return breadcrumbs;
        } 
        #endregion
    }
}
