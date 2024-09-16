using Oqtane.Models;
using Oqtane.UI;

namespace ToSic.Cre8magic.Client.Models;

public static class MagicPageExtensions
{
    public static MagicPage ToMagicPage(this Page page, PageState pageState) => new(page, pageState);

    public static IEnumerable<MagicPage> ToMagicPages(this IEnumerable<Page> pages, PageState pageState) => pages.Select(page => page.ToMagicPage(pageState));
}