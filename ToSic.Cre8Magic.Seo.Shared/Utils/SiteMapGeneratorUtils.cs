using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;

namespace ToSic.Cre8magic.Seo.Shared.Utils
{
    public class SiteMapGeneratorUtils
    {
        public const string SitemapXml = "sitemap.xml";
        public static string SiteMapUrl(Alias alias) => $"{alias!.Protocol}{alias.Name}/{SitemapXml}";
        public static string PageUrl(Alias alias, Page page) => $"{alias.Protocol}{alias.Name}/{page.Path}";
        public static string GenerateSiteMapXml(Alias alias, IEnumerable<Page> pages)
        {
            var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
            var urlset = new XElement(ns + "urlset");
            var siteMapDocument = new XDocument(urlset);
            var modified = DateTime.UtcNow.ToString("o");
            foreach (var page in SiteMapPages(pages))
            {
                urlset.Add(new XElement
                    (ns + "url",
                        new XElement(ns + "loc", SiteMapGeneratorUtils.PageUrl(alias, page)),
                        new XElement(ns + "lastmod", modified /* page.ModifiedOn */)
                    )
                );
            }

            return $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{siteMapDocument}";
        }

        private static IEnumerable<Page> SiteMapPages(IEnumerable<Page> pages) =>
            pages.Where(page => !page.IsDeleted && page.IsNavigation)
                .Where(page => UserSecurity.IsAuthorized(null, PermissionNames.View, page.Permissions))
                .OrderBy(p => p.Level).ThenBy(p => p.Order)
                .ToList();
    }
}
