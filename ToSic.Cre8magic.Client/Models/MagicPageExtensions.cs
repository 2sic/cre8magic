using Oqtane.Models;
using Oqtane.UI;

namespace ToSic.Cre8magic.Client.Models;

public static class MagicPageExtensions
{
    public static MagicPage ToMagicPage(this Page page, MagicPageService magicPageService) => new MagicPage(page).Init(magicPageService);

    public static MagicPage ToMagicPage(this Page page, PageState pageState) => new MagicPage(page).Init(new MagicPageService().Init(pageState));

    public static IEnumerable<MagicPage> ToMagicPages(this IEnumerable<Page> pages, PageState pageState) => pages.Select(page => page.ToMagicPage(pageState));
}