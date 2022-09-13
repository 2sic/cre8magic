using Oqtane.Models;
using Oqtane.UI;
using ToSic.Oqt.Cre8Magic.Client.Styling;
using static ToSic.Oqt.Cre8Magic.Client.MagicPlaceholders;

namespace ToSic.Oqt.Cre8Magic.Client.Tokens
{
    internal class ModuleTokens: PagePlaceholders
    {

        public ModuleTokens(PageState pageState, Module module) : base(pageState, null, null)
        {
            _module = module;
        }
        private readonly Module _module;

        /// <summary>
        /// Standard replace for strings
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string? Replace(string? value)
        {
            if (!value.HasValue()) return value;
            var mod = value!
                    .Replace(ModuleId, $"{_module.ModuleId}")
                    .ConditionalReplace(ModuleControlName, () => NamespaceParts[^1])
                    .ConditionalReplace(ModuleNamespace, () => string.Join('.', NamespaceParts[..^1]))
                ;

            return mod!.Contains(PlaceholderMarker) 
                ? base.Replace(mod) 
                : mod;
        }

        private string[] NamespaceParts => _module.ModuleType.Split(',')[0].Split('.');

        /// <summary>
        /// Replace for container design rules
        /// </summary>
        /// <param name="styles"></param>
        /// <returns></returns>
        public string Replace(MagicContainerDesign styles)
        {
            var value =  string.Join(" ", new[]
            {
                styles.Classes,
                _module.IsPublished() ? styles.IsPublished : styles.IsNotPublished, // Info-Class if not published
                _module.UseAdminContainer ? styles.IsAdminModule : styles.IsNotAdminModule // Info-class if admin module
            }.Where(s => s.HasValue()));

            return value.Contains(PlaceholderMarker)
                ? Replace(value)
                : value;
        }
    }
}
