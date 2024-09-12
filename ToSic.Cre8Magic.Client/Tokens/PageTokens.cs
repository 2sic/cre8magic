using Oqtane.UI;
using ToSic.Cre8magic.Client.Models;
using static System.StringComparison;
using static ToSic.Cre8magic.Client.MagicTokens;

namespace ToSic.Cre8magic.Client.Tokens;

internal class PageTokens: ITokenReplace
{
    public const string NameIdConstant = nameof(PageTokens);
    public PageState PageState { get; }
    public MagicPage? Page { get; }
    private readonly string? _bodyClasses;
    private readonly string? _menuId;
    public string NameId => NameIdConstant;

    public PageTokens(PageState pageState, MagicPage? page = null, string? bodyClasses = null, string? menuId = null)
    {
        PageState = pageState;
        Page = page;
        _bodyClasses = bodyClasses;
        _menuId = menuId;
    }

    public PageTokens Modified(MagicPage page, string? menuId = null) => new(PageState, page, _bodyClasses, menuId ?? _menuId);

    public string Parse(string classes)
    {
        if (!classes.HasValue()) return classes;
        var page = Page ?? PageState.Page.ToMagicPage();
        var result = classes
            .Replace(PageId, $"{page.PageId}", InvariantCultureIgnoreCase);

        // If there are no placeholders left, exit
        if (!result.Contains(PlaceholderMarker)) return result;

        result = result
            .Replace(PageParentId, page.ParentId != null ? $"{page.ParentId}" : None)
            .Replace(SiteId, $"{page.SiteId}", InvariantCultureIgnoreCase)
            .Replace(LayoutVariation, _bodyClasses ?? None)
            .Replace(MenuLevel, $"{page.Level + 1}")
            .Replace(MenuId, _menuId ?? None);

        // Checking the breadcrumb is a bit more expensive, so be sure we need it
        if (result.Contains(PageRootId))
            result = result
                .Replace(PageRootId, CurrentPageRootId != null ? $"{CurrentPageRootId}" : None);

        return result;
    }

    private int? CurrentPageRootId
    {
        get
        {
            if (_pageRootAlreadyTried) return _pageRootId;
            _pageRootAlreadyTried = true;
            _pageRootId = PageState.Breadcrumbs().FirstOrDefault()?.PageId;
            return _pageRootId;
        }
    }

    private int? _pageRootId;
    private bool _pageRootAlreadyTried;
}