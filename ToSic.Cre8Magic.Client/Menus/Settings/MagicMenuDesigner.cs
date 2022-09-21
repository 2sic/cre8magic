using System.Collections;

namespace ToSic.Oqt.Cre8Magic.Client.Menus.Settings;

/// <summary>
/// Special helper to provide Css classes to menus
/// </summary>
internal class MagicMenuDesigner
{
    public MagicMenuDesigner(MagicMenuSettings menuConfig)
    {
        MenuSettings = menuConfig as MagicMenuSettings ?? throw new ArgumentException("MenuConfig must be real", nameof(MenuSettings));

        DesignSettingsList = new() { MenuSettings.DesignSettings! };
    }
    private MagicMenuSettings MenuSettings { get; }
    internal List<MagicMenuDesignSettings> DesignSettingsList { get; }

    public string Value(string key, MagicMenuBranch branch)
    {
        var configsForKey = ConfigsForTag(key)
            .Select(c => c.Value)
            .Where(v => v.HasValue())
            .ToList();

        return string.Join(" ", configsForKey);
    }

    public string Classes(string tag, MagicMenuBranch branch)
    {
        var configsForTag = ConfigsForTag(tag);
        return configsForTag.Any()
            ? ListToClasses(TagClasses(branch, configsForTag as List<MagicMenuDesign>))
            : "";
    }

    private List<MagicMenuDesign> ConfigsForTag(string tag) =>
        DesignSettingsList
            .Select(c => c.FindInvariant(tag))
            .Where(c => c is { })
            .ToList()!;

    private List<string?> TagClasses(MagicMenuBranch branch, List<MagicMenuDesign> configs)
    {
        var classes = new List<string?>();
        classes.AddRange(configs.Select(c => c.Classes));
        classes.AddRange(configs.Select(c => c.IsActive.Get(branch.IsActive)));
        classes.AddRange(configs.Select(c => c.HasChildren.Get(branch.HasChildren)));
        classes.AddRange(configs.Select(c => c.IsDisabled.Get(!branch.Page.IsClickable)));
        classes.AddRange(configs.Select(c => c.InBreadcrumb.Get(branch.InBreadcrumb)));

        // See if there are any css for this level or for not-specified levels
        var levelCss = configs
            .Select(c => c.ByLevel == null
                ? null
                : c.ByLevel.TryGetValue(branch.MenuLevel, out var levelClasses)
                    ? levelClasses
                    : c.ByLevel.TryGetValue(MagicTokens.ByLevelOtherKey, out var levelClassesDefault)
                        ? levelClassesDefault
                        : null);
        classes.AddRange(levelCss);

        return classes;
    }



    private string ListToClasses(IEnumerable<string?> original)
        => string.Join(" ", original.Where(s => !s.IsNullOrEmpty())).Replace("  ", " ");
}