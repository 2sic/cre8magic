using static ToSic.Cre8Magic.Client.MagicConstants;
using static ToSic.Cre8Magic.Client.Settings.Json.JsonMerger;

namespace ToSic.Cre8Magic.Client.Services;

internal class NamedSettingsReader<TPart> where TPart: class, new()
{
    public NamedSettingsReader(
        MagicSettingsService parent,
        Defaults<TPart> defaults,
        Func<MagicSettingsCatalog, NamedSettings<TPart>> findList,
        Func<string, Func<string, string>>? jsonProcessing = null)
    {
        _parent = parent;
        _defaults = defaults;
        _findList = findList;
        _jsonProcessing = jsonProcessing;
    }
    private readonly MagicSettingsService _parent;
    private readonly Defaults<TPart> _defaults;
    private readonly Func<MagicSettingsCatalog, NamedSettings<TPart>> _findList;
    private readonly Func<string, Func<string, string>>? _jsonProcessing;

    internal TPart Find(string name, string? defaultName = null)
    {
        var names = GetConfigNamesToCheck(name, defaultName ?? name);
        var realName = names[0];
        var cached = _cache.FindInvariant(realName);
        if (cached != null) return cached;

        // Get best part; return Fallback if nothing found
        var priority = FindPart(names);
        if (priority == null) return _defaults.Fallback;

        // Check if our part declares that it inherits something
        if (priority is IInherit needsMore && needsMore.Inherits.HasText())
        {
            var inheritFrom = needsMore.Inherits;
            needsMore.Inherits = null;
            var addition = FindPart(inheritFrom);
            if (addition != null)
                priority = Merge(priority, addition, _jsonProcessing?.Invoke(realName));
        }

        if (_defaults.Foundation == null) return priority;

        var merged = Merge(priority, _defaults.Foundation, _jsonProcessing?.Invoke(realName));
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

    internal TPart? FindPart(params string[]? names)
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