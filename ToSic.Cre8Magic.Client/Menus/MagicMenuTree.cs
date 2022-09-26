using System.Diagnostics.CodeAnalysis;
using Oqtane.Models;
using Oqtane.UI;
using ToSic.Cre8Magic.Client.OqtanePatches;

namespace ToSic.Cre8Magic.Client.Menus;

public class MagicMenuTree : MagicMenuBranch
{
    public const char PageForced = '!';

    public MagicMenuTree(MagicSettings magicSettings, MagicMenuSettings settings, List<Page> menuPages, List<string> debug, IHasSettingsExceptions exceptions)
        : base(null! /* root must be null, as `Tree` is handled in this class */, 0, magicSettings.PageState.Page, "Root")
    {
        LogChild = new(Log, "Root");
        MagicSettings = magicSettings;
        PageState = magicSettings.PageState;
        Settings = settings;
        AllPages = magicSettings.PageState.Pages;
        MenuPages = menuPages;
        _exceptions = exceptions;
        Debug = debug ?? new();

        // Bug in Oqtane 3.2 and before: Level isn't hydrated
        if (AllPages.All(p => p.Level == 0))
            MenuPatchCode.GetPagesHierarchy(AllPages);
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

    public override List<Exception> Exceptions => _exceptions.Exceptions;
    private readonly IHasSettingsExceptions _exceptions;

    protected override MagicMenuTree Tree => this;

    internal MagicMenuDesigner Design => _menuCss ??= new(Settings);
    private MagicMenuDesigner? _menuCss;

    internal List<Page> Breadcrumb => _breadcrumb ??= AllPages.Breadcrumb(Page).ToList();
    private List<Page>? _breadcrumb;

    public override string MenuId => _menuId ??= (Settings as MagicMenuSettings)?.MenuId ?? "error-menu-id";
    private string? _menuId;

    public List<string> Debug { get; }

    internal Logging.Log Log { get; } = new();

    [return: NotNull]
    protected override List<Page> GetChildPages()
    {
        // Give empty list if we shouldn't display it
        if (Settings.Display == false)
        {
            LogChild.A("Display == false, don't show");
            return new();
        }

        // Case 1: StartPage *, so all top-level entries
        var start = Settings.Start?.Trim();

        // Case 2: '.' - not yet tested
        var startLevel = Settings.Level ?? MagicMenuSettings.StartLevelFallback;
        var getChildren = Settings.Children ?? MagicMenuSettings.ChildrenFallback;
        var startingPoints = ConfigToStartingPoints(start, startLevel, getChildren);
        // Case 3: one or more IDs to start from

        var startPages = FindStartPages(startingPoints);

        return startPages;
    }

    internal List<Page> FindStartPages(StartingPoint[] pageIds)
    {
        var result = pageIds.SelectMany(FindStartingPage)
            .Where(p => p != null)
            .ToList();
        return result;
    }

    private IEnumerable<Page> FindStartingPage(StartingPoint n)
    {
        var source = n.Force ? Tree.AllPages : Tree.MenuPages;

        // Start by getting all the anchors - the pages to start from, before we know about children or not
        // Three cases: root, current, ...
        var anchors = n.Id != default
            ? source.Where(p => p.PageId == n.Id).ToList()
            : n.From == MagicMenuSettings.StartPageRoot
                ? source.Where(p => p.Level == 0).ToList()
                : null;

        if (anchors == null && n.From == MagicMenuSettings.StartPageCurrent)
            // Level 0 means current level / current page
            if (n.Level == 0)
                anchors = new() { Page };
            // Level 1 means top-level pages. If we don't want the level1 children, we want the top-level items
            // TODO: CHECK WHAT LEVEL Oqtane actually gives us, is 1 the top?
            else if (n.Level == 1 && !n.Children)
                anchors = source.Where(p => p.Level == 0).ToList();
            else
            {
                var ancestors = source.GetAncestors(Page);
                if (n.Level > 0) ancestors = ancestors.Reverse();
                // If coming from the top, level 1 means top level, so skip one less
                var level = n.Level > 0 ? n.Level - 1 : Math.Abs(n.Level);
                anchors = ancestors.Skip(level).ToList();
            }

        anchors ??= new();

        return n.Children
            ? anchors.SelectMany(p => GetRelatedPagesByLevel(p, 1)).ToList()
            : anchors;
    }


    private List<Page> GetRelatedPagesByLevel(Page referencePage, int level)
    {
        switch (level)
        {
            case -1:
                return ChildrenOf(referencePage.ParentId ?? 0);
            case 0:
                return new() { referencePage };
            case 1:
                return ChildrenOf(referencePage.PageId);
            case > 1:
                return new() { ErrPage(0, "Error: Create menu from current page but level > 1") };
            default:
                return new() { ErrPage(0, "Error: Create menu from current page but level < -1, not yet implemented") };
        }
    }

    private StartingPoint[] ConfigToStartingPoints(string? value, int level, bool children)
    {
        LogChild.Call($"{nameof(value)}: '{value}'; {nameof(level)}: {level}; {nameof(children)}: {children}");

        if (!value.HasText()) return Array.Empty<StartingPoint>();
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
                return new StartingPoint { Id = id, From = fromNode, Force = important, Children = children, Level = level};
            })
            .Where(n => n != null)
            .ToArray();
        return result as StartingPoint[];
    }
}