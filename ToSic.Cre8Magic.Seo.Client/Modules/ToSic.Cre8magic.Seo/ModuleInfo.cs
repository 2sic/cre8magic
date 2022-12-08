using Oqtane.Models;
using Oqtane.Modules;

namespace ToSic.Cre8magic.Seo
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Cre8magicSeo",
            Description = "Cre8magicSeo",
            Version = "1.0.0",
            ServerManagerType = "ToSic.Cre8magic.Seo.Manager.Cre8magic.SeoManager, ToSic.Cre8magic.Seo.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "ToSic.Cre8magic.Seo.Shared.Oqtane",
            PackageName = "ToSic.Cre8magic.Seo",
            Categories = "Admin"
        };
    }
}
