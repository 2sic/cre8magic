using System.Diagnostics.CodeAnalysis;
using Oqtane.Models;
using Log = ToSic.Cre8Magic.Client.Logging.Log;

namespace ToSic.Cre8Magic.Client.Menus;

public class MagicMenuBranch //: IHasSettingsExceptions
{
    /// <summary>
    /// Root navigator object which has some data/logs for all navigators which spawned from it. 
    /// </summary>
    protected virtual MagicMenuTree Tree { get; }

    private ITokenReplace NodeReplace => _nodeReplace ??= Tree.PageTokenEngine(Page);
    private ITokenReplace? _nodeReplace;

    public string? Classes(string tag) => NodeReplace.Parse(Tree.Design.Classes(tag, this)).EmptyAsNull();

    public string? Value(string key) => NodeReplace.Parse(Tree.Design.Value(key, this)).EmptyAsNull();

    /// <summary>
    /// Menu Level relative to the start of the menu (always starts with 1)
    /// </summary>
    public int MenuLevel { get; }

    public MagicMenuBranch(MagicMenuTree tree, int menuLevel, Page page, string debugPrefix)
    {
        if (tree != null)
        {
            Log = tree.LogRoot.GetLog(debugPrefix);
            Log.A($"Branch for {nameof(page)}: {page.PageId}; {nameof(menuLevel)}: {menuLevel}");
        }
        Tree = tree!;
        Page = page;
        MenuLevel = menuLevel;
    }

    /// <summary>
    /// Current Page
    /// </summary>
    public Page Page { get; }

    internal Log Log { get; set; }

    protected string LogPageList(List<Page>? pages) =>
        pages == null ? "" : string.Join(",", pages.Select(p => p.PageId)) + $"(total: {pages.Count})";

    public bool HasChildren => Children.Any();

    public bool IsActive => Page.PageId == Tree.Page.PageId;

    public bool InBreadcrumb => Tree.Breadcrumb.Contains(Page);

    public virtual string MenuId => Tree.MenuId;

    public IList<MagicMenuBranch> Children => _children ??= GetChildren();
    private IList<MagicMenuBranch>? _children;

    /// <summary>
    /// Retrieve the children the first time it's needed.
    /// </summary>
    /// <returns></returns>
    [return: NotNull]
    protected List<MagicMenuBranch> GetChildren()
    {
        var l = Log.Fn<List<MagicMenuBranch>>($"{nameof(MenuLevel)}: {MenuLevel}");
        var levelsRemaining = (Tree.Settings.Depth ?? MagicMenuSettings.LevelDepthFallback) - MenuLevel + 1;
        if (levelsRemaining <= 0)
            return l.Return(new(), "no levels remaining, empty");
        
        var children = GetChildPages()
            .Select(page => new MagicMenuBranch(Tree, MenuLevel + 1, page, $"{Log.Prefix}>{Page.PageId}"))
            .ToList();
        return l.Return(children, $"{children.Count}");
    }


    protected virtual List<Page> GetChildPages()
    {
        var l = Log.Fn<List<Page>>();
        if (Page == null)
            return l.Return(new() { ErrPage(-1, "Error: No current page found") }, "no current page found");

        var result = ChildrenOf(Page.PageId);
        return l.Return(result, LogPageList(result));
    }

    protected List<Page> ChildrenOf(int pageId)
    {
        var l = Log.Fn<List<Page>>(pageId.ToString());
        var result = Tree.MenuPages.Where(p => p.ParentId == pageId).ToList();
        return l.Return(result, LogPageList(result));
    }

    //protected List<Page> FindPages(int[] pageIds)
    //    => Tree.MenuPages.Where(p => pageIds.Contains(p.PageId)).ToList();


    protected static Page ErrPage(int id, string message) => new() { PageId = id, Name = message };

    //public virtual List<Exception> Exceptions => Tree.Exceptions;
}