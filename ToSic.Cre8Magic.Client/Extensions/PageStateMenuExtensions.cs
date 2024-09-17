using Oqtane.UI;
using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client;

public static class PageStateMenuExtensions
{
    internal static MagicPage? GetHomePage(this PageState pageState) => pageState.Pages.Find(p => p.Path == "")?.ToMagicPage();

    internal static bool CurrentPageIsHome(this PageState pageState) => pageState?.Page.Path == "";

    internal static List<MagicPage> Breadcrumbs(this PageState pageState, MagicPage? page = null)
        => GetAncestors(pageState, page).Reverse().ToList();

    internal static List<MagicPage> Breadcrumbs(this List<MagicPage> pages, MagicPage page)
        => GetAncestors(pages, page).Reverse().ToList();

    internal static List<MagicPage> Ancestors(this PageState pageState, MagicPage? page = null)
        => GetAncestors(pageState, page).ToList();

    private static IEnumerable<MagicPage> GetAncestors(PageState pageState, MagicPage? page = null) 
        => GetAncestors(pageState.Pages.ToMagicPages().ToList(), page ?? pageState.Page.ToMagicPage());

    internal static IEnumerable<MagicPage> GetAncestors(this List<MagicPage> pages, MagicPage? page)
    {
        while (page != null)
        {
            yield return page;
            page = pages.FirstOrDefault(p => p.PageId == page.ParentId);
        }
    }
}