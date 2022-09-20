using System.Text.Json;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;
using ToSic.Oqt.Cre8Magic.Client.Settings.JsonMerge;
using static ToSic.Oqt.Cre8Magic.Client.MagicConstants;

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

    public MagicDebugSettings Debug => _debug
        ??= ConfigurationSources.FirstOrDefault(c => c.Debug != null)?.Debug
            ?? MagicDebugSettings.Defaults.Fallback;
    private MagicDebugSettings? _debug;

    internal ThemeDesigner ThemeDesigner { get; } = new();


    private MagicPackageSettings PackageSettings
    {
        get => _settings ?? throw new ArgumentException($"The {nameof(MagicSettingsService)} can't work without first calling {nameof(InitSettings)}", nameof(PackageSettings));
        set => _settings = value;
    }
    private MagicPackageSettings? _settings;

    private MagicSettingsJsonService Json { get; }

    public MagicSettings CurrentSettings(PageState pageState, string? name, string bodyClasses)
    {
        // Get a cache-id for this specific configuration, which can vary by page
        var originalNameForCache = (name ?? "prevent-error") + pageState.Page.PageId;
        var cached = _currentSettingsCache.FindInvariant(originalNameForCache);
        if (cached != null) return cached;

        // Tokens engine for this specific PageState
        var tokens = new TokenEngine(new()
        {
            new PageTokens(pageState, null, bodyClasses),
            new ThemeTokens(PackageSettings)
        });

        // Figure out real config-name, and get the initial layout
        var configName = FindConfigName(name, Default);
        name = configName.ConfigName;
        var layout = Layout.Find(name).Parse(tokens);

        var current = new MagicSettings(name, this, layout, tokens, pageState);
        ThemeDesigner.InitSettings(current);
        current.MagicContext = ThemeDesigner.BodyClasses(pageState, tokens);
        var dbg = current.DebugSources;
        dbg.Add("Name", configName.Source);

        _currentSettingsCache[originalNameForCache] = current;
        return current;
    }

    internal MagicSettingsCatalog MergedCatalog => _mergedCatalog ??= MergeCatalogs();
    private MagicSettingsCatalog? _mergedCatalog;

    private MagicSettingsCatalog MergeCatalogs()
    {
        var sources = ConfigurationSources;
        var priority = JsonSerializer.Serialize(sources.First());
        foreach (var source in sources.Skip(1))
        {
            // get new json
            var lowerPriority = JsonSerializer.Serialize(source, JsonMerger.OptionsForPreMerge);
            var merged = JsonMerger.Merge(priority, lowerPriority);
            priority = merged;
        }
        var result = JsonSerializer.Deserialize<MagicSettingsCatalog>(priority);
        return result!;
    }

    private readonly NamedSettings<MagicSettings> _currentSettingsCache = new();

    private NamedSettingsReader<MagicThemeSettings> Layout => _getLayout ??=
        new(this, MagicThemeSettings.Defaults, cat => cat.Themes,
            (name) => json => json.Replace("\"=\"", $"\"{name}\""));
    private NamedSettingsReader<MagicThemeSettings>? _getLayout;

    internal NamedSettingsReader<MagicBreadcrumbSettings> Breadcrumbs => _getBreadcrumbs ??=
        new(this, MagicBreadcrumbSettings.Defaults, cat => cat.Breadcrumbs);
    private NamedSettingsReader<MagicBreadcrumbSettings>? _getBreadcrumbs;


    internal NamedSettingsReader<MagicMenuSettings> MenuSettings => _getMenuSettings ??=
        new(this, MagicMenuSettings.Defaults, cat => cat.Menus);
    private NamedSettingsReader<MagicMenuSettings>? _getMenuSettings;

    internal NamedSettingsReader<MagicLanguagesSettings> Languages => _languages ??=
        new(this, MagicLanguagesSettings.Defaults, cat => cat.Languages);
    private NamedSettingsReader<MagicLanguagesSettings>? _languages;

    internal NamedSettingsReader<MagicLanguageDesignSettings> LanguageDesign => _languageDesign ??=
        new(this, MagicLanguageDesignSettings.Defaults, cat => cat.LanguageDesigns);
    private NamedSettingsReader<MagicLanguageDesignSettings>? _languageDesign;

    internal NamedSettingsReader<MagicContainerSettings> Containers => _containers ??=
        new(this, MagicContainerSettings.Defaults, cat => cat.Containers);
    private NamedSettingsReader<MagicContainerSettings>? _containers;

    internal NamedSettingsReader<MagicContainerDesignSettings> ContainerDesign => _containerDesign ??=
        new(this, MagicContainerDesignSettings.Defaults, cat => cat.ContainerDesigns);
    private NamedSettingsReader<MagicContainerDesignSettings>? _containerDesign;

    internal NamedSettingsReader<MagicThemeDesignSettings> ThemeDesign => _themeDesign ??=
        new(this, MagicThemeDesignSettings.Defaults, cat => cat.ThemeDesigns);
    private NamedSettingsReader<MagicThemeDesignSettings>? _themeDesign;

    internal NamedSettingsReader<MagicMenuDesignSettings> MenuDesigns => _menuDesigns ??=
        new(this, MagicMenuDesignSettings.Defaults, cat => cat.MenuDesigns);
    private NamedSettingsReader<MagicMenuDesignSettings>? _menuDesigns;


    internal (string ConfigName, string Source) FindConfigName(string? configName, string inheritedName)
    {
        var debugInfo = $"Initial Config: '{configName}'";
        if (configName.EqInvariant(InheritName))
        {
            configName = inheritedName;
            debugInfo += $"; switched to inherit '{inheritedName}'";
        }
        if (configName.HasText()) return (configName, debugInfo);

        debugInfo += $"; Config changed to '{Default}'";
        return (Default, debugInfo);
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