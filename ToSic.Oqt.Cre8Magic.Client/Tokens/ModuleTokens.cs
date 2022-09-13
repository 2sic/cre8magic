using Oqtane.Models;
using Oqtane.UI;

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
    }
}
