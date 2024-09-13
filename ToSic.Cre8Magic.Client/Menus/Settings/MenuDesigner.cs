using System.Collections;

namespace ToSic.Cre8magic.Client.Menus.Settings;

/// <summary>
/// Special helper to provide Css classes to menus
/// </summary>
internal class MenuDesigner : IMenuDesigner
{
    public MenuDesigner(MagicMenuTree tree, MagicMenuSettings menuConfig)
    {
        MenuSettings = menuConfig ?? throw new ArgumentException("MenuConfig must be real", nameof(MenuSettings));

        DesignSettingsList = new() { MenuSettings.DesignSettings! };

        Log = menuConfig.Debug?.Detailed == true ? tree.LogRoot.GetLog("MenuDesigner") : null;
    }
    private MagicMenuSettings MenuSettings { get; }
    internal List<NamedSettings<MagicMenuDesign>> DesignSettingsList { get; }
    private ILog? Log { get; }

    public string Classes(string tag, MagicMenuPage page)
    {
        var l = Log.Fn<string>($"{nameof(tag)}: {tag}, page: {page.Page.PageId} \"{page.Page.Name}\"");
        var configsForTag = ConfigsForTag(tag);
        var result = configsForTag.Any()
            ? ListToClasses(TagClasses(page, configsForTag))
            : "";
        return l.ReturnAndLog(result);
    }

    public string Value(string key, MagicMenuPage page)
    {
        var l = Log.Fn<string>(key);
        var configsForKey = ConfigsForTag(key)
            .Select(c => c.Value)
            .Where(v => v.HasValue())
            .ToList();

        return l.ReturnAndLog(string.Join(" ", configsForKey));
    }

    private List<MagicMenuDesign> ConfigsForTag(string tag) =>
        DesignSettingsList
            .Select(c => c.FindInvariant(tag))
            .Where(c => c is { })
            .ToList()!;

    private List<string?> TagClasses(MagicMenuPage page, IReadOnlyCollection<MagicMenuDesign> configs)
    {
        var classes = new List<string?>();

        void AddIfAny(IEnumerable<string?> maybeAdd)
        {
            var additions = maybeAdd.Where(v => v != null).ToList();
            if (additions.Any()) classes.AddRange(additions);
        }

        AddIfAny(configs.Select(c => c.Classes));
        AddIfAny(configs.Select(c => c.Classes));
        AddIfAny(configs.Select(c => c.IsActive.Get(page.IsActive)));
        AddIfAny(configs.Select(c => c.HasChildren.Get(page.HasChildren)));
        AddIfAny(configs.Select(c => c.IsDisabled.Get(!page.Page.IsClickable)));
        AddIfAny(configs.Select(c => c.InBreadcrumb.Get(page.InBreadcrumb)));

        // See if there are any css for this level or for not-specified levels
        var levelCss = configs
            .Select(c => c.ByLevel == null
                ? null
                : c.ByLevel.TryGetValue(page.Level, out var levelClasses)
                    ? levelClasses
                    : c.ByLevel.TryGetValue(MagicTokens.ByLevelOtherKey, out var levelClassesDefault)
                        ? levelClassesDefault
                        : null);
        AddIfAny(levelCss);

        return classes;
    }



    private string ListToClasses(IEnumerable<string?> original)
        => string.Join(" ", original.Where(s => !s.IsNullOrEmpty())).Replace("  ", " ");
}