using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using ToSic.Cre8magic.Seo.Server.Services;
using ToSic.Cre8magic.Seo.Shared.Utils;

namespace ToSic.Cre8magic.Seo.Server.Middleware
{
    public class GoogleSiteMapMiddleware
    {

        private readonly RequestDelegate _next;

        public GoogleSiteMapMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // fast url check
            if (!context.Request.Path.ToString().EndsWith(SiteMapGeneratorUtils.SitemapXml, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // need Alias for detail url check
            var tenantResolver = context.RequestServices.GetRequiredService<ITenantResolver>();
            var alias = tenantResolver.GetAlias();
            if (alias == null)
            {
                await _next(context);
                return; // skip when alias is missing
            }

            // detail url check
            if (!CurrentUrl(context).Equals(SiteMapGeneratorUtils.SiteMapUrl(alias), StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return; // skip when detail url is not as expected
            }

            // generate the sitemap and write it to the response
            var siteMapGenerator = context.RequestServices.GetRequiredService<SiteMapGeneratorService>();
            var sitemap = siteMapGenerator.GenerateSiteMapDocument(context);
            context.Response.ContentType = "application/xml";
            await context.Response.WriteAsync(sitemap);
        }

        private static string CurrentUrl(HttpContext context) => $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
    }
}
