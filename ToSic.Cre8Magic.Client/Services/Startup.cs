using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ToSic.Cre8magic.Client.Analytics;

namespace ToSic.Cre8magic.Client.Services;

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

        //services.TryAddTransient<MagicPageService>(); // Can't DI because of PageState dependency that breaks Oqtane 
        //services.TryAddTransient<MagicMenuTree>(); // Can't DI because of PageState dependency that breaks Oqtane 
    }
}
