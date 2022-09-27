using System.IO.Compression;
using Oqtane.Models;
using Oqtane.UI;

namespace ToSic.Cre8Magic.Client.Menus;

public class MagicMenuTree : MagicMenuBranch
{
    public const char PageForced = '!';

    public MagicMenuTree(MagicSettings magicSettings, MagicMenuSettings settings, List<Page> menuPages, List<string> messages)
        : base(magicSettings.PageState.Page, 0)
    {
        Log = LogRoot.GetLog("Root");
        Log.A($"Start for Page: {magicSettings.PageState.Page.PageId}; Level: 0");
        MagicSettings = magicSettings;
        PageState = magicSettings.PageState;
        Settings = settings;
        AllPages = magicSettings.PageState.Pages;
        MenuPages = menuPages;
        Debug = messages ?? new();
    }

    public MagicMenuSettings Settings { get; }
    private MagicSettings MagicSettings { get; }
    public PageState PageState { get; }

    internal TokenEngine PageTokenEngine(Page page)
    {
        var originalPage = (PageTokens)MagicSettings.Tokens.Parsers.First(p => p.NameId == PageTokens.NameIdConstant);
        originalPage = originalPage.Modified(page, menuId: MenuId);
        return MagicSettings.Tokens.SwapParser(originalPage);
    }

    /// <summary>
    /// List of all pages - even these which would currently not be shown in the menu.
    /// </summary>
    internal List<Page> AllPages { get; }

    /// <summary>
    /// Pages in the menu according to Oqtane pre-processing
    /// Should be limited to pages which should be in the menu, visible and permissions ok. 
    /// </summary>
    internal List<Page> MenuPages;

    protected override MagicMenuTree Tree => this;

    internal MagicMenuDesigner Design => _menuCss ??= new(Settings);
    private MagicMenuDesigner? _menuCss;

    internal List<Page> Breadcrumb => _breadcrumb ??= AllPages.Breadcrumbs(Page).ToList();
    private List<Page>? _breadcrumb;

    public override string MenuId => _menuId ??= Settings?.MenuId ?? "error-menu-id";
    private string? _menuId;

    public List<string> Debug { get; }

    internal Logging.LogRoot LogRoot { get; } = new();

    protected override List<Page> GetChildPages() => GetRootPages();

    protected List<Page> GetRootPages()
    {
        var l = Log.Fn<List<Page>>($"{Page.PageId}");
        // Give empty list if we shouldn't display it
        if (Settings.Display == false)
            return l.Return(new(), "Display == false, don't show");

        // Case 1: StartPage *, so all top-level entries
        var start = Settings.Start?.Trim();

        // Case 2: '.' - not yet tested
        var startLevel = Settings.Level ?? MagicMenuSettings.StartLevelFallback;
        var getChildren = Settings.Children ?? MagicMenuSettings.ChildrenFallback;
        var startingPoints = GetStartNodeRules(start, startLevel, getChildren);
        // Case 3: one or more IDs to start from

        var startPages = FindStartPageOfManyRules(startingPoints);

        return l.Return(startPages, LogPageList(startPages));
    }

    internal List<Page> FindStartPageOfManyRules(StartNodeRule[] startingPoints)
    {
        var l = Log.Fn<List<Page>>(string.Join(',', startingPoints.Select(p => p.Id)));
        var result = startingPoints.SelectMany(FindStartPagesOfOneRule)
            .Where(p => p != null)
            .ToList();
        return l.Return(result, LogPageList(result));
    }

    private List<Page> FindStartPagesOfOneRule(StartNodeRule n)
    {
        var l = Log.Fn<List<Page>>($"Include hidden pages: {n.Force}; Mode: {n.ModeInfo}");

        // Start by getting all the anchors - the pages to start from, before we know about children or not
        // Three cases: root, current, ...
        var anchorPages = FindInitialAnchorPages(n);

        var result = n.ShowChildren
            ? anchorPages.SelectMany(p => GetRelatedPagesByLevel(p, 1)).ToList()
            : anchorPages;

        return l.Return(result, LogPageList(result));
    }

    private List<Page> FindInitialAnchorPages(StartNodeRule n)
    {
        var l = Log.Fn<List<Page>>();
        var source = n.Force ? Tree.AllPages : Tree.MenuPages;
        switch (n.ModeInfo)
        {
            case StartMode.PageId:
                return l.Return(source.Where(p => p.PageId == n.Id).ToList(), $"Page with id {n.Id}");
            case StartMode.Root:
                return l.Return(source.Where(p => p.Level == 0).ToList(), "Home/root");
            // Level 0 means current level / current page
            case StartMode.Current when n.Level == 0:
                return l.Return(new() { Page }, "Current page");
            // Level 1 means top-level pages. If we don't want the level1 children, we want the top-level items
            // TODO: CHECK WHAT LEVEL Oqtane actually gives us, is 1 the top?
            case StartMode.Current when n.Level == 1 && !n.ShowChildren:
                return l.Return(source.Where(p => p.Level == 0).ToList(), "Current level 1?");
            case StartMode.Current when n.Level > 0:
                // If coming from the top, level 1 means top level, so skip one less
                var skipDown = n.Level - 1;
                var fromTop = source.Breadcrumbs(Page).Skip(skipDown).FirstOrDefault();
                var fromTopResult = fromTop == null ? new() : new List<Page> { fromTop };
                return l.Return(fromTopResult, $"from root to breadcrumb by {skipDown}");
            case StartMode.Current when n.Level < 0:
                // If going up, must change skip to normal
                var skipUp = Math.Abs(n.Level);
                var fromCurrent = source.GetAncestors(Page).ToList().Skip(skipUp).FirstOrDefault();
                var result = fromCurrent == null ? new() : new List<Page> { fromCurrent };
                return l.Return(result, $"up the ancestors by {skipUp}");
            default:
                return l.Return(new(), "nothing");
        }
    }


    private List<Page> GetRelatedPagesByLevel(Page referencePage, int level)
    {
        var l = Log.Fn<List<Page>>($"{referencePage.PageId}; {level}");
        List<Page> result;
        switch (level)
        {
            case -1:
                result = ChildrenOf(referencePage.ParentId ?? 0);
                return l.Return(result, "siblings - " + LogPageList(result));
            case 0:
                result = new() { referencePage };
                return l.Return(result, "current page - " + LogPageList(result));
            case 1:
                result = ChildrenOf(referencePage.PageId);
                return l.Return(result, "children - " + LogPageList(result));
            case > 1:
                return l.Return(new() { ErrPage(0, "Error: Create menu from current page but level > 1") }, "err");
            default:
                return l.Return(new() { ErrPage(0, "Error: Create menu from current page but level < -1, not yet implemented") }, "err");
        }
    }

    private StartNodeRule[] GetStartNodeRules(string? value, int level, bool showChildren)
    {
        var l = Log.Fn<StartNodeRule[]>($"{nameof(value)}: '{value}'; {nameof(level)}: {level}; {nameof(showChildren)}: {showChildren}");

        if (!value.HasText()) return l.Return(Array.Empty<StartNodeRule>(), "no value, empty list");
        var parts = value.Split(',');
        var result = parts
            .Select(fromNode =>
            {
                fromNode = fromNode.Trim();
                if (!fromNode.HasText()) return null;
                var important = fromNode.EndsWith(PageForced);
                if (important) fromNode = fromNode.TrimEnd(PageForced);
                fromNode = fromNode.Trim();
                int.TryParse(fromNode, out var id);
                return new StartNodeRule { Id = id, From = fromNode, Force = important, ShowChildren = showChildren, Level = level};
            })
            .Where(n => n != null)
            .ToArray() as StartNodeRule[];
        return l.ReturnAndKeepData(result, result.Length.ToString());
    }
}