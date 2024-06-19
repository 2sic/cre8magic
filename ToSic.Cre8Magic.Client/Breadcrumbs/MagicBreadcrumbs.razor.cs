using Oqtane.Models;

namespace ToSic.Cre8magic.Client.Breadcrumbs;

public abstract class MagicBreadcrumbs: MagicControl
{
    // The home page - never changes during runtime, so we can cache it
    protected Page HomePage => _homePage ??= PageState.GetHomePage()!;
    private Page? _homePage;

    protected List<Page> Breadcrumbs
    {
        get
        {
            // Reset cache if the page has changed
            if (_lastPageId != PageState.Page.PageId) _breadcrumbs = null;
            _lastPageId = PageState.Page.PageId;
            // Return cached or new breadcrumbs
            return _breadcrumbs ??= PageState.Breadcrumbs();
        }
    }

    private int? _lastPageId;
    private List<Page>? _breadcrumbs;
}