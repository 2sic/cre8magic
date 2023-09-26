using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ToSic.Cre8Magic.Client.Analytics;

namespace ToSic.Cre8Magic.Client.Services;

public class Startup : Oqtane.Services.IClientStartup
{
    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // All these Settings etc. should be scoped, so they don't have to reload for each click
        services.TryAddScoped<MagicSettingsJsonService>();
        services.TryAddScoped<MagicSettingsService>();
        services.TryAddTransient<LanguageService>();

        services.TryAddTransient<MagicThemeJsServiceTest>();

        // Logic parts for Controls
        services.TryAddTransient<MagicPageEditService>();

        // Analytics - new in 0.0.2
        services.TryAddTransient<MagicAnalyticsService>();

        services.TryAddTransient<MagicMenuBuilder>();
    }
}
