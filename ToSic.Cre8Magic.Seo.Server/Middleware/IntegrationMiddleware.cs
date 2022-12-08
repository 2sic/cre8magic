using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ToSic.Cre8magic.Seo.Server.Services;

namespace ToSic.Cre8magic.Seo.Server.Middleware
{
    public class IntegrationMiddleware
    {
        public IntegrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            // ensure admin/sitemap page after other middlewares are executed
            if (!SkipOrExecuted(context))
            {
                var integrationService = context.RequestServices.GetRequiredService<IntegrationService>();
                var siteId = integrationService.CreateAdminPage();
                if (siteId > 0) _isExecuted.TryAdd(siteId, true);
            }
        }

        public bool SkipOrExecuted(HttpContext context)
        {
            if (context.Request.Path.ToString().StartsWith("/_blazor")) 
                return true; // just skip for Blazor

            var alias = context.Items.ContainsKey(Constants.HttpContextAliasKey) ? context.Items[Constants.HttpContextAliasKey] as Alias : null;
            if (alias != null) 
                return _isExecuted.ContainsKey(alias.SiteId);

            alias = context.RequestServices.GetRequiredService<ITenantResolver>().GetAlias();
            if (alias == null) 
                return true; // just skip because Alias is missing

            // save alias in HttpContext (this should happen after TenantMiddleware is invoked)
            context.Items[Constants.HttpContextAliasKey] =  alias;

            return _isExecuted.ContainsKey(alias.SiteId);
        }
        private readonly ConcurrentDictionary<int, bool> _isExecuted = new(); // use only key, ignore value (aka concurrent hash set)
    }
}
