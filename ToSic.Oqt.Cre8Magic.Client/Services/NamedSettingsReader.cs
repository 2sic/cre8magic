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

        var priority = _parent.FindInMerged((set, n) => _findList(set).GetInvariant(n), names);

        if (priority == null) return _defaults.Fallback;
        if (_defaults.Foundation == null) return priority;

        var merged = JsonMerger.Merge(priority, _defaults.Foundation, _optionalJsonProcessing?.Invoke(realName));
        return merged!;
    }

    private readonly NamedSettings<TPart> _cache = new();

    private static string[] GetConfigNamesToCheck(string? configuredNameOrNull, string currentName)
    {
        if (configuredNameOrNull == Inherit) configuredNameOrNull = currentName;

        return string.IsNullOrWhiteSpace(configuredNameOrNull)
            ? new[] { Default }
            : new[] { configuredNameOrNull, Default }.Distinct().ToArray();
    }

}