using System.Text.Json;
using Oqtane.UI;
using ToSic.Cre8Magic.Client.Settings.Json;
using static ToSic.Cre8Magic.Client.MagicConstants;

namespace ToSic.Cre8Magic.Client.Services;

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
        var configDetails = FindConfigName(name, Default);
        name = configDetails.ConfigName;
        var theme = Theme.Find(name).Parse(tokens);

        var current = new MagicSettings(name, this, theme, tokens, pageState);
        //ThemeDesigner.InitSettings(current);
        current.MagicContext = current.ThemeDesigner.BodyClasses(tokens);
        var dbg = current.DebugSources;
        dbg.Add("Name", string.Join("; ", configDetails.Source));

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

    private NamedSettingsReader<MagicThemeSettings> Theme => _getTheme ??=
        new(this, MagicThemeSettings.Defaults, cat => cat.Themes,
            (name) => json => json.Replace("\"=\"", $"\"{name}\""));
    private NamedSettingsReader<MagicThemeSettings>? _getTheme;

    internal NamedSettingsReader<MagicMenuSettings> MenuSettings => _getMenuSettings ??=
        new(this, MagicMenuSettings.Defaults, cat => cat.Menus);
    private NamedSettingsReader<MagicMenuSettings>? _getMenuSettings;

    internal NamedSettingsReader<MagicLanguagesSettings> Languages => _languages ??=
        new(this, MagicLanguagesSettings.Defaults, cat => cat.Languages);
    private NamedSettingsReader<MagicLanguagesSettings>? _languages;

    internal NamedSettingsReader<MagicLanguagesDesignSettings> LanguagesDesign => _languageDesign ??=
        new(this, MagicLanguagesDesignSettings.Defaults, cat => cat.LanguagesDesigns);
    private NamedSettingsReader<MagicLanguagesDesignSettings>? _languageDesign;

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


    internal (string ConfigName, List<string> Source) FindConfigName(string? configName, string inheritedName)
    {
        var debugInfo = new List<string> { $"Initial Config: '{configName}'"};
        if (configName.EqInvariant(InheritName))
        {
            configName = inheritedName;
            debugInfo.Add($"switched to inherit '{inheritedName}'");
        }
        if (configName.HasText()) return (configName, debugInfo);

        debugInfo.Add($"Config changed to '{Default}'");
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