using Microsoft.Extensions.DependencyInjection;
using Oqtane.Extensions;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ToSic.Cre8magic.Seo.Server.Services
{
    public class IntegrationService
    {
        private const string SiteMapPageName = "SiteMap Management";

        public IntegrationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public int CreateAdminPage()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var tenantResolver = scope.ServiceProvider.GetRequiredService<ITenantResolver>();
            var alias = tenantResolver.GetAlias();
            if (alias == null) return -1;

            var pageRepository = scope.ServiceProvider.GetRequiredService<IPageRepository>();
            var pages = pageRepository.GetPages(alias.SiteId);
            if (pages.Any(p => p.Name == SiteMapPageName)) return alias.SiteId;

            var sites = scope.ServiceProvider.GetRequiredService<ISiteRepository>();
            var site = sites.GetSite(alias.SiteId);
            if (site == null) return -1;

            sites.CreatePages(site, AdminSiteMapPage(), alias);
            return alias.SiteId;
        }

        private static List<PageTemplate> AdminSiteMapPage() =>
            new()
            {
                new PageTemplate
                {
                    Name = SiteMapPageName,
                    Parent = "Admin",
                    Order = 1111,
                    Path = "admin/sitemap",
                    Icon = Icons.Map,
                    IsNavigation = false,
                    IsPersonalizable = false,
                    PagePermissions = new List<Permission>
                    {
                        new Permission(PermissionNames.View, RoleNames.Admin, true),
                        new Permission(PermissionNames.Edit, RoleNames.Admin, true)
                    }.EncodePermissions(),
                    PageTemplateModules = new List<PageTemplateModule>
                    {
                        new PageTemplateModule
                        {
                            ModuleDefinitionName = "ToSic.Cre8magic.Seo, ToSic.Cre8magic.Seo.Client.Oqtane",
                            Title = "SiteMap Management",
                            Pane = PaneNames.Default,
                            ModulePermissions = new List<Permission>
                            {
                                new Permission(PermissionNames.View, RoleNames.Admin, true),
                                new Permission(PermissionNames.Edit, RoleNames.Admin, true)
                            }.EncodePermissions(),
                            Content = ""
                        }
                    }
                }
            };
    }
}
