using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Modules;
using ToSic.Cre8magic.Seo.Server.Services;

// ReSharper disable once CheckNamespace
namespace ToSic.Cre8magic.Seo.Manager
{
    public class SeoManager : MigratableModuleBase, IInstallable, IPortable
    {
        private readonly IntegrationService _integrationService;

        public SeoManager(IntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        public bool Install(Tenant tenant, string version)
        {
            // TODO: STV test, it looks that this is not executed as expected
            //_integrationService.CreateAdminPage();
            return true;
        }

        public bool Uninstall(Tenant tenant)
        {
            // TODO: delete admin page "SiteMap Management"
            return false;
        }

        public string ExportModule(Module module)
        {
            return string.Empty;
        }

        public void ImportModule(Module module, string content, string version)
        {
            //throw new System.NotImplementedException();
        }
    }
}
