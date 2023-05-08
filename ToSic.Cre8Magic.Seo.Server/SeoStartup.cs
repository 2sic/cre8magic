using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oqtane.Infrastructure;
using ToSic.Cre8magic.Seo.Server.Middleware;
using ToSic.Cre8magic.Seo.Server.Services;

namespace ToSic.Cre8magic.Seo.Server
{
    public class SeoStartup : IServerStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<SiteMapGeneratorService>();
            services.TryAddSingleton<IntegrationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<IntegrationMiddleware>();
            app.UseMiddleware<GoogleSiteMapMiddleware>();
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            //throw new NotImplementedException();
        }
    }
}
