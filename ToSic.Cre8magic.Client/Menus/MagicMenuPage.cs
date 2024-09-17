using Oqtane.UI;
using System.Diagnostics.CodeAnalysis;
using ToSic.Cre8magic.Client.Models;
using Log = ToSic.Cre8magic.Client.Logging.Log;

namespace ToSic.Cre8magic.Client.Menus;

/// <summary>
/// Represents a menu page in the MagicMenu system.
/// </summary>
/// <remarks>
/// Can't provide PageState from DI because that breaks Oqtane.
/// </remarks>
public class MagicMenuPage : MagicPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MagicMenuPage"/> class.
    /// </summary>
    /// <param name="page">The original page.</param>
    /// <param name="level">The menu level.</param>
    /// <param name="pageState">The page state.</param>
    /// <param name="tree">The magic menu tree.</param>
    /// <param name="debugPrefix">The debug prefix.</param>
    protected MagicMenuPage(MagicPage page, int level, PageState pageState, MagicMenuTree tree = null, string debugPrefix = null) : base(page.OriginalPage)
    {
        Level = level;

        PageState = pageState;
        MagicPageService = new MagicPageService(pageState);

        if (tree == null) return;
        Tree = tree;
        Log = tree.LogRoot.GetLog(debugPrefix);
        var _ = PageInfo;   // Access page info early on to make logging nicer
    }

    /// <summary>
    /// This service provides functionality for the menu control.
    /// It is based on the core 'oqtane.framework\Oqtane.Client\Themes\Controls\Theme\MenuBase.cs'
    /// but it favors composition over inheritance.
    /// </summary>
    private protected readonly MagicPageService MagicPageService;

    /// <summary>
    /// PageState id dependency that provides information about the current page,
    /// also it is used by derived classes MagicMenuPage, MagicMenuThree
    /// </summary>
    protected PageState PageState { get; }

    /// <summary>
    /// Menu Level relative to the start of the menu (always starts with 1)
    /// </summary>
    public int Level { get; protected init; }

    /// <summary>
    /// Root navigator object which has some data/logs for all navigators which spawned from it. 
    /// </summary>
    internal virtual MagicMenuTree Tree { get; }

    private ITokenReplace NodeReplace => _nodeReplace ??= Tree.PageTokenEngine(this);
    private ITokenReplace? _nodeReplace;

    /// <summary>
    /// Get css class for tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public string? Classes(string tag) => NodeReplace.Parse(Tree.Design.Classes(tag, this)).EmptyAsNull();

    /// <summary>
    /// Get attribute value.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? Value(string key) => NodeReplace.Parse(Tree.Design.Value(key, this)).EmptyAsNull();

    internal Log Log { get; set; }

    protected string LogPageList(List<MagicPage>? pages) =>
        pages?.Any() == true ? $"{pages.Count} pages [" + string.Join(",", pages.Select(p => p.PageId)) + "]" : "(no pages)";

    /// <summary>
    /// Special central place to get, cache and log the special properties only once
    /// </summary>
    internal MagicPageInfo PageInfo
    {
        get
        {
            if (_pI != null) return _pI;
            var l = Log.Fn<MagicPageInfo>($"Page: {PageId}");
            _pI = new()
            {
                HasChildren = Children.Any(),
                IsActive = PageId == Tree.PageId,
                InBreadcrumb = Tree.Breadcrumb.Contains(this),
            };
            return l.Return(_pI, $"Name: '{Name}': {_pI.Log}");
        }
    }

    private MagicPageInfo? _pI;

    /// <summary>
    /// Determines if there are sub-pages. True if this page has sub-pages.
    /// </summary>
    public bool HasChildren => PageInfo.HasChildren;

    /// <summary>
    /// Determine if the menu page is current page.
    /// </summary>
    public bool IsActive => PageInfo.IsActive;

    /// <summary>
    /// Determine if the menu page is in the breadcrumb.
    /// </summary>
    public bool InBreadcrumb => PageInfo.InBreadcrumb;

    /// <summary>
    /// The ID of the menu item
    /// </summary>
    public virtual string MenuId => Tree.MenuId;

    /// <summary>
    /// Link to this page.
    /// </summary>
    public string Link => MagicPageService.GetUrl(this);

    /// <summary>
    /// Target for link to this page.
    /// </summary>
    public string Target => MagicPageService.GetTarget(this);

    /// <summary>
    /// Get children of the current menu page.
    /// </summary>
    public IList<MagicMenuPage> Children => _children ??= GetChildren();
    private IList<MagicMenuPage>? _children;

    /// <summary>
    /// Retrieve the children the first time it's needed.
    /// </summary>
    /// <returns></returns>
    [return: NotNull]
    protected List<MagicMenuPage> GetChildren()
    {
        var l = Log.Fn<List<MagicMenuPage>>($"{nameof(Level)}: {Level}");
        var levelsRemaining = Tree.Depth - (Level - 1 /* Level is 1 based, so -1 */);
        if (levelsRemaining < 0)
            return l.Return(new(), "remaining levels 0 - return empty");

        var children = GetChildPages()
            .Select(page => new MagicMenuPage(page, Level + 1, PageState, Tree, $"{Log.Prefix}>{PageId}"))
            .ToList();
        return l.Return(children, $"{children.Count}");
    }

    private const string ErrPageNotFound = "Error: Page not found";

    protected virtual List<MagicPage> GetChildPages()
    {
        var l = Log.Fn<List<MagicPage>>();

        var result = ChildrenOf(PageId);
        return l.Return(result, LogPageList(result));
    }

    protected List<MagicPage> ChildrenOf(int pageId)
    {
        var l = Log.Fn<List<MagicPage>>(pageId.ToString());
        var result = Tree.MenuPages.Where(p => p.ParentId == pageId).ToList();
        return l.Return(result, LogPageList(result));
    }

    //protected List<Page> FindPages(int[] pageIds)
    //    => Tree.MenuPages.Where(p => pageIds.Contains(p.PageId)).ToList();


    protected MagicPage ErrPage(int id, string message) => new(new() { PageId = id, Name = message });
}