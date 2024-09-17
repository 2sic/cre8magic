using Oqtane.Models;

namespace ToSic.Cre8magic.Client.Models;

public static class MagicPageExtensions
{
    public static MagicPage ToMagicPage(this Page page) => new(page);

    public static IEnumerable<MagicPage> ToMagicPages(this IEnumerable<Page> pages) => pages.Select(page => page.ToMagicPage());
}