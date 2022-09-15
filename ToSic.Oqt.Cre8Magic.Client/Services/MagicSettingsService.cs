using System.Text.Json;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;
using ToSic.Oqt.Cre8Magic.Client.Settings.JsonMerge;
using static ToSic.Oqt.Cre8Magic.Client.MagicConstants;
using static ToSic.Oqt.Cre8Magic.Client.Settings.MagicPackageSettings;

namespace ToSic.Oqt.Cre8Magic.Client.Services;

/// <summary>
/// Service which consolidates settings made in the UI, in the JSON and falls back to coded defaults.
/// </summary>
public class MagicSettingsService: IHasSettingsExceptions
{
    /// <summary>
    /// Constructor
    /// </summary>
    public MagicSettingsService(MagicSettingsJsonService jsonService)
    {
        Json = jsonService;
    }

    public MagicSettingsService InitSettings(MagicPackageSettings themeSettings)
    {
        PackageSettings = themeSettings;
        return this;
    }

    public bool Debug => ConfigurationSources.First().Debug;

    internal ThemeDesigner ThemeDesigner { get; } = new();


    private MagicPackageSettings PackageSettings
    {
        get => _settings ?? throw new ArgumentException($"The {nameof(MagicSettingsService)} can't work without first calling {nameof(InitSettings)}", nameof(PackageSettings));
        set => _settings = value;
    }
    private MagicPackageSettings _settings;

    private MagicSettingsJsonService Json { get; }

    public MagicSettings CurrentSettings(PageState pageState, string name, string bodyClasses)
    {
        var tokensPro = new TokenEngine(new()
        {
            new PageTokens(pageState, null, bodyClasses),
            new ThemeTokens(PackageSettings)
        });

        // Get a cache-id for this specific configuration, which can vary by page
        var originalNameForCache = (name ?? "prevent-error") + pageState.Page.PageId;
        var cached = _currentSettingsCache.FindInvariant(originalNameForCache);
        if (cached != null) return cached;

        // Figure out real config-name, and get the initial layout
        var configName = FindConfigName(name, Default);
        name = configName.ConfigName;
        var layout = Layout.Find(name).Parse(tokensPro);

        var current = new MagicSettings(name, this, layout, tokensPro, pageState);
        ThemeDesigner.InitSettings(current);
        current.MagicContext = ThemeDesigner.BodyClasses(pageState, tokensPro);
        var dbg = current.DebugSources;
        dbg.Add("Name", configName.Source);

        _currentSettingsCache[originalNameForCache] = current;
        return current;
    }

    private MagicSettingsCatalog MergedCatalog => _mergedCatalog ??= MergeCatalogs();
    private MagicSettingsCatalog? _mergedCatalog;

    private MagicSettingsCatalog MergeCatalogs()
    {
        var sources = ConfigurationSources;
        var priority = JsonSerializer.Serialize(sources.First());
        foreach (var source in sources.Skip(1))
        {
            // get new json
            var lowerPriority = JsonSerializer.Serialize(source, JsonMerger.OptionsForPreMerge);
            var merged = JsonMerger.Merge(lowerPriority, priority);
            priority = merged;
        }
        var result = JsonSerializer.Deserialize<MagicSettingsCatalog>(priority);
        return result!;
    }

    private readonly NamedSettings<MagicSettings> _currentSettingsCache = new();

    private NamedSettingsReader<MagicThemeSettings> Layout => _getLayout ??=
        new(this, MagicThemeSettings.Defaults,
            (set, n) => set.Layouts?.GetInvariant(n),
            (name) => json => json.Replace("\"=\"", $"\"{name}\""));
    private NamedSettingsReader<MagicThemeSettings>? _getLayout;

    internal NamedSettingsReader<MagicBreadcrumbSettings> Breadcrumbs => _getBreadcrumbs ??=
        new(this, MagicBreadcrumbSettings.Defaults,
            (set, n) => set.Breadcrumbs?.GetInvariant(n));
    private NamedSettingsReader<MagicBreadcrumbSettings>? _getBreadcrumbs;


    internal NamedSettingsReader<MagicMenuSettings> MenuSettings => _getMenuSettings ??=
        new(this, MagicMenuSettings.Defaults,
            (set, n) => set.Menus?.GetInvariant(n));
    private NamedSettingsReader<MagicMenuSettings>? _getMenuSettings;

    internal NamedSettingsReader<MagicLanguagesSettings> Languages => _languages ??=
        new(this, MagicLanguagesSettings.Defaults,
            (set, n) => set.Languages?.GetInvariant(n));
    private NamedSettingsReader<MagicLanguagesSettings>? _languages;

    internal NamedSettingsReader<MagicLanguageDesignSettings> LanguageDesign => _languageDesign ??=
        new(this, MagicLanguageDesignSettings.Defaults,
            (set, n) => set.LanguageDesigns?.GetInvariant(n));
    private NamedSettingsReader<MagicLanguageDesignSettings>? _languageDesign;

    internal NamedSettingsReader<MagicContainerSettings> Containers => _containers ??=
        new(this, MagicContainerSettings.Defaults,
            (set, n) => set.Containers?.GetInvariant(n));
    private NamedSettingsReader<MagicContainerSettings>? _containers;

    internal NamedSettingsReader<MagicContainerDesignSettings> ContainerDesign => _containerDesign ??=
        new(this, MagicContainerDesignSettings.Defaults,
            (set, n) => set.ContainerDesigns?.GetInvariant(n));
    private NamedSettingsReader<MagicContainerDesignSettings>? _containerDesign;

    internal NamedSettingsReader<MagicThemeDesignSettings> ThemeDesign => _themeDesign ??=
        new(this, MagicThemeDesignSettings.Defaults,
            (set, n) => set.PageDesigns?.GetInvariant(n));
    private NamedSettingsReader<MagicThemeDesignSettings>? _themeDesign;

    internal NamedSettingsReader<MagicMenuDesignSettings> MenuDesigns => _menuDesigns ??=
        new(this, MagicMenuDesignSettings.Defaults,
            (set, n) => set.MenuDesigns?.GetInvariant(n));
    private NamedSettingsReader<MagicMenuDesignSettings>? _menuDesigns;


    internal (string ConfigName, string Source) FindConfigName(string? configName, string inheritedName)
    {
        var debugInfo = $"Initial Config: '{configName}'";
        if (configName.EqInvariant(Inherit))
        {
            configName = inheritedName;
            debugInfo += $"; switched to inherit '{inheritedName}'";
        }
        if (!string.IsNullOrWhiteSpace(configName)) return (configName, debugInfo);

        debugInfo += $"; Config changed to '{Default}'";
        return (Default, debugInfo);
    }


    
    internal TResult? FindInMerged<TResult>(Func<MagicSettingsCatalog, string, TResult> findFunc, params string[]? names)
    {
        // Make sure we have at least one name
        if (names == null || names.Length == 0) names = new[] { Default };

        var allSourcesAndNames = names
            .Distinct()
            .Select(name => (Settings: MergedCatalog, Name: name))
            .ToList();

        foreach (var set in allSourcesAndNames)
        {
            var result = findFunc(set.Settings, set.Name);
            if (result != null) return result;
        }

        return default;
    }


    private List<MagicSettingsCatalog> ConfigurationSources
    {
        get
        {
            if (_configurationSources != null) return _configurationSources;
            var sources = new List<MagicSettingsCatalog?>
                {
                    // in future also add the settings from the dialog as the first priority
                    Json.LoadJson(PackageSettings),
                    PackageSettings.Defaults,
                    Fallback.Defaults,
                }
                .Where(x => x != null)
                .Cast<MagicSettingsCatalog>()
                .ToList();
            return _configurationSources = sources;
        }
    }

    private List<MagicSettingsCatalog>? _configurationSources;

    public List<Exception> Exceptions => MyExceptions.Concat(Json.Exceptions).ToList();
    private List<SettingsException> MyExceptions { get; } = new();
}