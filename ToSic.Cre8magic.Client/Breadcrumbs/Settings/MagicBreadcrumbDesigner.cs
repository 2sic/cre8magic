using System.Collections;

namespace ToSic.Cre8magic.Client.Breadcrumbs.Settings;

/// <summary>
/// Special helper to provide Css classes to menus
/// </summary>
public class MagicBreadcrumbDesigner : IBreadcrumbDesigner
{
    public MagicBreadcrumbDesigner(MagicBreadcrumb breadcrumb, MagicBreadcrumbSettings breadcrumbConfig)
    {
        BreadcrumbSettings = breadcrumbConfig ?? throw new ArgumentException("BreadcrumbConfig must be real", nameof(BreadcrumbSettings));

        DesignSettingsList = new() { BreadcrumbSettings.DesignSettings! };
    }
    private MagicBreadcrumbSettings BreadcrumbSettings { get; }
    internal List<NamedSettings<MagicBreadcrumbDesign>> DesignSettingsList { get; }


    public string Classes(string tag, MagicBreadcrumbItem item)
    {
        var configsForTag = ConfigsForTag(tag);
        return configsForTag.Any()
            ? ListToClasses(TagClasses(item, configsForTag))
            : "";
    }

    public string Value(string key, MagicBreadcrumbItem item)
    {
        var configsForKey = ConfigsForTag(key)
            .Select(c => c.Value)
            .Where(v => v.HasValue())
            .ToList();

        return string.Join(" ", configsForKey);
    }

    private List<MagicBreadcrumbDesign> ConfigsForTag(string tag) =>
        DesignSettingsList
            .Select(c => c.FindInvariant(tag))
            .Where(c => c is { })
            .ToList()!;

    private List<string?> TagClasses(MagicBreadcrumbItem page, IReadOnlyCollection<MagicBreadcrumbDesign> configs)
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
        AddIfAny(configs.Select(c => c.IsDisabled.Get(!page.IsClickable)));
        //AddIfAny(configs.Select(c => c.InBreadcrumb.Get(page.InBreadcrumb)));

        //// See if there are any css for this level or for not-specified levels
        //var levelCss = configs
        //    .Select(c => c.ByLevel == null
        //        ? null
        //        : c.ByLevel.TryGetValue(page.Level, out var levelClasses)
        //            ? levelClasses
        //            : c.ByLevel.TryGetValue(MagicTokens.ByLevelOtherKey, out var levelClassesDefault)
        //                ? levelClassesDefault
        //                : null);
        //AddIfAny(levelCss);

        return classes;
    }

    private string ListToClasses(IEnumerable<string?> original)
        => string.Join(" ", original.Where(s => !s.IsNullOrEmpty())).Replace("  ", " ");
}