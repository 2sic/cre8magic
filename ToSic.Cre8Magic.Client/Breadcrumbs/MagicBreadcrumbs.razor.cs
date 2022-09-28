using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Breadcrumbs;

public abstract class MagicBreadcrumbs: MagicControl
{
    // The home page - never changes during runtime, so we can cache it
    protected Page HomePage => _homePage ??= PageState.GetHomePage()!;
    private Page? _homePage;

    protected List<Page> Breadcrumbs => _breadcrumbs ??= PageState.Breadcrumbs();
    private List<Page>? _breadcrumbs;
}