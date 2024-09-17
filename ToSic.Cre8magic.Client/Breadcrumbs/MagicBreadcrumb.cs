using Oqtane.UI;
//using ToSic.Cre8magic.Client.Breadcrumbs.Settings;
using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client.Breadcrumbs
{
    public class MagicBreadcrumb(PageState pageState)
    {

        public MagicBreadcrumb Setup(MagicPage currentPage)
        {
            _currentPage = currentPage;
            return this;
        }

        //public MagicBreadcrumb Designer(IBreadcrumbDesigner designer)
        //{
        //    Design = designer;
        //    return this;
        //}
        //internal IBreadcrumbDesigner Design { get; private set; }

        public MagicBreadcrumb Setup(int currentPageId)
        {
            _currentPage = pageState.Pages.FirstOrDefault(p => p.PageId == currentPageId)?.ToMagicPage() ?? _homePage;
            return this;
        }

        public List<MagicPage> Breadcrumbs => GetBreadcrumbs();
        //public List<MagicPage> Breadcrumbs => pageState.Breadcrumbs(_currentPage).ToList();

        #region Private Methods
        private MagicPage _currentPage = pageState.Page.ToMagicPage();

        private readonly MagicPage _homePage = pageState.Pages.First(p => p.Path == "").ToMagicPage();

        private List<MagicPage> GetBreadcrumbs(MagicPage? page = null)
        {
            var currentPage = page ?? _currentPage;
            var breadcrumbs = new List<MagicPage>() { currentPage };

            if (_homePage.PageId == currentPage.PageId) return breadcrumbs;

            breadcrumbs.Insert(0, _homePage);

            var parentPage = pageState.Pages.FirstOrDefault(p => p.PageId == currentPage.ParentId);
            while (parentPage != null && _homePage.PageId != parentPage.PageId)
            {
                breadcrumbs.Insert(1, parentPage!.ToMagicPage());
                parentPage = pageState.Pages.FirstOrDefault(p => p.PageId == parentPage.ParentId);
            }
            return breadcrumbs;
        } 
        #endregion
    }
}
