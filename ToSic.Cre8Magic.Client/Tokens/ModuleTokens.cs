using Oqtane.Models;
using static ToSic.Cre8Magic.Client.MagicTokens;

namespace ToSic.Cre8Magic.Client.Tokens;

internal class ModuleTokens: ITokenReplace
{
    private const string NameIdConstant = nameof(ModuleTokens);

    public ModuleTokens(Module module) => _module = module;
    private readonly Module _module;

    public string NameId => NameIdConstant;

    /// <summary>
    /// Standard replace for strings
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string? Parse(string? value)
    {
        if (!value.HasValue()) return value;
        var mod = value!
                .Replace(ModuleId, $"{_module.ModuleId}")
                .Replace(ModuleControlName, () => NamespaceParts[^1])
                .Replace(ModuleNamespace, () => string.Join('.', NamespaceParts[..^1]))
            ;
        return mod;
    }

    private string[] NamespaceParts => _module.ModuleType.Split(',')[0].Split('.');
}