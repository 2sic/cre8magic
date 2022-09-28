using Oqtane.Models;
using ToSic.Cre8Magic.Client.Settings;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

internal class ContainerDesigner: ThemeDesigner
{
    internal const string ModulePrefixDefault = "module";

    public ContainerDesigner(MagicSettings settings, Module module): base(settings) => _module = module;
    private readonly Module _module;


    // protected override DesignSetting? GetSettings(string name) => Settings?.ContainerDesign.GetInvariant(name);

    public override string? Classes(string tag)
    {
        if (GetSettings(tag) is not { } styles) return null;

        var value = GetClasses(styles);
        return PostProcess(value);
    }

    protected override TokenEngine Tokens => _tokens1 ??= Settings.Tokens.Expanded(new ModuleTokens(_module));
    private TokenEngine? _tokens1;


    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    private string GetClasses(DesignSetting styles)
    {
        var value =  string.Join(" ", new[]
        {
            styles.Classes,
            styles.IsPublished.Get(_module.IsPublished()),      // Info-Class if not published
            styles.IsAdmin.Get(_module.ForceAdminContainer())   // Info-class if admin module
        }.Where(s => s.HasValue()));

        return value;
    }
}