using System.Diagnostics.CodeAnalysis;
using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Menus;

public class MagicMenuBranch: IHasSettingsExceptions
{
    /// <summary>
    /// Root navigator object which has some data/logs for all navigators which spawned from it. 
    /// </summary>
    protected virtual MagicMenuTree Tree { get; }

    private ITokenReplace NodeReplace => _nodeReplace ??= Tree.PageTokenEngine(Page);
    private ITokenReplace? _nodeReplace;

    public string? Classes(string tag) => NodeReplace.Parse(Tree.Design.Classes(tag, this)).EmptyAsNull();

    public string? Value(string key) => NodeReplace.Parse(Tree.Design.Value(key, this)).EmptyAsNull();

    public virtual List<string> Debug => Tree.Debug;

    /// <summary>
    /// Current Page
    /// </summary>
    public Page Page { get; }

    /// <summary>
    /// Menu Level relative to the start of the menu (always starts with 1)
    /// </summary>
    public int MenuLevel { get; }

    public MagicMenuBranch(MagicMenuTree root, int menuLevel, Page page)
    {
        Tree = root;
        Page = page;
        MenuLevel = menuLevel;
    }

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
        var levelsRemaining = (Tree.Settings.Depth ?? MagicMenuSettings.LevelDepthFallback) - MenuLevel + 1;
        return levelsRemaining <= 0
            ? new()
            : GetChildPages()
                .Select(page => new MagicMenuBranch(Tree, MenuLevel + 1, page))
                .ToList();
    }


    protected virtual List<Page> GetChildPages() => Page == null
        ? new() { ErrPage(-1, "Error: No current page found") }
        : ChildrenOf(Page.PageId);

    protected List<Page> ChildrenOf(int pageId)
        => Tree.MenuPages.Where(p => p.ParentId == pageId).ToList();

    //protected List<Page> FindPages(int[] pageIds)
    //    => Tree.MenuPages.Where(p => pageIds.Contains(p.PageId)).ToList();


    protected static Page ErrPage(int id, string message) => new() { PageId = id, Name = message };

    public virtual List<Exception> Exceptions => Tree.Exceptions;
}