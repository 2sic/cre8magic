using Oqtane.Models;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Styling;

namespace ToSic.Oqt.Cre8Magic.Client.Tokens
{
    internal class ModuleTokens: PagePlaceholders
    {

        public ModuleTokens(PageState pageState, Module module) : base(pageState, null, null)
        {
            _module = module;
        }
        private readonly Module _module;

        internal string Replace(string value)
        {
            var mod = value.Replace(MagicPlaceholders.ModuleId, _module.ModuleId.ToString());
            return mod.Contains(MagicPlaceholders.PlaceholderMarker) 
                ? base.Replace(mod) 
                : mod;
        }

        public string Replace(MagicContainerDesign styles)
        {
            var value =  string.Join(" ", new[]
            {
                styles.Classes,
                _module.IsPublished() ? styles.IsPublished : styles.IsNotPublished, // Info-Class if not published
                _module.UseAdminContainer ? styles.IsAdminModule : styles.IsNotAdminModule // Info-class if admin module
            }.Where(s => s.HasValue()));

            return value.Contains(MagicPlaceholders.PlaceholderMarker)
                ? Replace(value)
                : value;
        }
    }
}
