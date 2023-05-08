using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Oqtane.Shared;
using Oqtane.Models;
using System.Security.Claims;
using Oqtane.Security;
using ToSic.Cre8magic.Seo.Shared.Utils;

namespace ToSic.Cre8magic.Seo.Server.Services
{
    public class SiteMapGeneratorService
    {
        public string GenerateSiteMapDocument(HttpContext context)
        {
            var tenantResolver = context.RequestServices.GetRequiredService<ITenantResolver>();
            var alias = tenantResolver.GetAlias();
            if (alias == null) throw new Exception("Alias is null");

            var pageRepository = context.RequestServices.GetRequiredService<IPageRepository>();
            var pages = pageRepository.GetPages(alias.SiteId);

            return SiteMapGeneratorUtils.GenerateSiteMapXml(alias, pages);
        }
    }
}
