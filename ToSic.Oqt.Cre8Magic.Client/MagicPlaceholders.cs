namespace ToSic.Oqt.Cre8Magic.Client;

public class MagicPlaceholders
{
    /// <summary>
    /// This will be used as value if a value is null/empty.
    /// For example, it would give a page-parent-none if there is no parent
    /// </summary>
    internal const string None = "none";
    internal const string PlaceholderMarker = "[";


    internal const string SiteId = "[Site.Id]";

    internal const string PageId = "[Page.Id]";
    internal const string PageParentId = "[Page.ParentId]";
    internal const string PageRootId = "[Page.RootId]";

    internal const string ModuleId = "[Module.Id]";
    internal const string ModuleControlName = "[Module.ControlName]";
    internal const string ModuleNamespace = "[Module.Namespace]";

    // TODO!
    internal const string ThemePath = "[Theme.Path]";
    internal const string ThemeAssetsPath = "[Theme.AssetsPath]";

    internal const string MenuId = "[Menu.Id]";
    internal const string MenuLevel = "[Menu.Level]";

    // TODO! naming
    internal const string LayoutVariation = "[Layout.Variation]";

    /// <summary>
    /// Special key to mark rules "ByLevel" which apply to all level which had not been defined
    /// </summary>
    internal const int ByLevelOtherKey = -1;
}