using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

internal class ContainerDesigner: ThemeDesigner
{
    public ContainerDesigner(MagicSettings settings, Module module): base(settings) => _module = module;
    private readonly Module _module;

    protected override TokenEngine Tokens => _tokens1 ??= Settings.Tokens.Expanded(new ModuleTokens(_module));
    private TokenEngine? _tokens1;

    public override string? Classes(string tag)
    {
        if (GetSettings(tag) is not { } styles) return null;
        var value = CombineWithModuleClasses(styles);
        return PostProcess(value);
    }


    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    private string CombineWithModuleClasses(DesignSetting styles)
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