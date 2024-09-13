using System.Diagnostics.CodeAnalysis;
using ToSic.Cre8magic.Client.Models;
using Log = ToSic.Cre8magic.Client.Logging.Log;

namespace ToSic.Cre8magic.Client.Menus;

public class MagicMenuPage
{
    /// <summary>
    /// Root navigator object which has some data/logs for all navigators which spawned from it. 
    /// </summary>
    internal virtual MagicMenuTree Tree { get; }

    private ITokenReplace NodeReplace => _nodeReplace ??= Tree.PageTokenEngine(Page);
    private ITokenReplace? _nodeReplace;

    public string? Classes(string tag) => NodeReplace.Parse(Tree.Design.Classes(tag, this)).EmptyAsNull();
    //private string? _lastClasses;

    public string? Value(string key) => NodeReplace.Parse(Tree.Design.Value(key, this)).EmptyAsNull();

    public MagicMenuPage(MagicMenuTree tree, int level, MagicPage page, string debugPrefix): this(page, level)
    {
        Tree = tree;
        Log = tree.LogRoot.GetLog(debugPrefix);
        var _ = PageInfo;   // Access page info early on to make logging nicer
    }

    protected MagicMenuPage(MagicPage page, int level)
    {
        Page = page;
        Level = level;
    }

    /// <summary>
    /// Current Page
    /// </summary>
    public MagicPage Page { get; protected set; }

    /// <summary>
    /// Menu Level relative to the start of the menu (always starts with 1)
    /// </summary>
    public int Level { get; protected set; }

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
            var l = Log.Fn<MagicPageInfo>($"Page: {Page.PageId}");
            _pI = new()
            {
                HasChildren = Children.Any(),
                IsActive = Page.PageId == Tree.Page.PageId,
                InBreadcrumb = Tree.Breadcrumb.Contains(Page),
            };
            return l.Return(_pI, $"Name: '{Page.Name}': {_pI.Log}"); 
        }
    }

    private MagicPageInfo? _pI;

    public bool HasChildren => PageInfo.HasChildren;

    public bool IsActive => PageInfo.IsActive;

    public bool InBreadcrumb => PageInfo.InBreadcrumb;

    public virtual string MenuId => Tree.MenuId;

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
            .Select(page => new MagicMenuPage(Tree, Level + 1, page, $"{Log.Prefix}>{Page.PageId}"))
            .ToList();
        return l.Return(children, $"{children.Count}");
    }

    private const string ErrPageNotFound = "Error: Page not found";

    protected virtual List<MagicPage> GetChildPages()
    {
        var l = Log.Fn<List<MagicPage>>();
        if (Page == null)
            return l.Return(new() { ErrPage(-1, ErrPageNotFound) }, ErrPageNotFound);

        var result = ChildrenOf(Page.PageId);
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


    protected static MagicPage ErrPage(int id, string message) => new(new() { PageId = id, Name = message });
}