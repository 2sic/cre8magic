using ToSic.Oqt.Cre8Magic.Client.Settings.JsonMerge;
using static ToSic.Oqt.Cre8Magic.Client.MagicConstants;

namespace ToSic.Oqt.Cre8Magic.Client.Services;

internal class NamedSettingsReader<TPart> where TPart: class, new()
{
    public NamedSettingsReader(
        MagicSettingsService parent,
        Defaults<TPart> defaults,
        Func<MagicSettingsCatalog, NamedSettings<TPart>> findList,
        Func<string, Func<string, string>>? optionalJsonProcessing = null)
    {
        _parent = parent;
        _defaults = defaults;
        _findList = findList;
        _optionalJsonProcessing = optionalJsonProcessing;
    }
    private readonly MagicSettingsService _parent;
    private readonly Defaults<TPart> _defaults;
    private readonly Func<MagicSettingsCatalog, NamedSettings<TPart>> _findList;
    private readonly Func<string, Func<string, string>>? _optionalJsonProcessing;

    internal TPart Find(string name, string? defaultName = null)
    {
        var names = GetConfigNamesToCheck(name, defaultName ?? name);
        var realName = names[0];
        var cached = _cache.FindInvariant(realName);
        if (cached != null) return cached;

        var priority = FindInMerged(names);

        if (priority == null) return _defaults.Fallback;

        if (priority is IInherit needsMore && needsMore.Inherits.HasText())
        {
            var addition = FindInMerged(needsMore.Inherits);
            if (addition != null)
                priority = JsonMerger.Merge(priority, addition, _optionalJsonProcessing?.Invoke(realName));
        }

        if (_defaults.Foundation == null) return priority;

        var merged = JsonMerger.Merge(priority, _defaults.Foundation, _optionalJsonProcessing?.Invoke(realName));
        return merged!;
    }

    private readonly NamedSettings<TPart> _cache = new();

    private static string[] GetConfigNamesToCheck(string? configuredNameOrNull, string currentName)
    {
        if (configuredNameOrNull == InheritName) configuredNameOrNull = currentName;

        return configuredNameOrNull.HasText()
            ? new[] { configuredNameOrNull, Default }.Distinct().ToArray()
            : new[] { Default };
    }

    internal TPart? FindInMerged(params string[]? names)
    {
        // Make sure we have at least one name
        if (names == null || names.Length == 0) names = new[] { Default };

        var allSourcesAndNames = names
            .Distinct()
            .Select(name => (Settings: _parent.MergedCatalog, Name: name))
            .ToList();

        foreach (var set in allSourcesAndNames)
        {
            var result = _findList(set.Settings).GetInvariant(set.Name);
            if (result != null) return result;
        }

        return default;
    }


}