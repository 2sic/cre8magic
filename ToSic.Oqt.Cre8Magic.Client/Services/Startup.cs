using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ToSic.Oqt.Cre8Magic.Client.Services;

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
        services.TryAddScoped<MagicSettingsService, MagicSettingsServiceMerge>();
        services.TryAddTransient<LanguageService>();

        //services.AddTransient<MenuTreeService>();

        //// Logic parts for Controls
        services.TryAddTransient<MagicPageEditService>();
        
    }
}
