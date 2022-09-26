using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

internal class ContainerDesigner: MagicDesignerBase
{
    internal const string ModulePrefixDefault = "module";

    public ContainerDesigner(MagicSettings settings, Module module)
    {
        InitSettings(settings);
        _module = module;
    }

    private readonly Module _module;


    protected override DesignSettingBase? GetSettings(string name) => Settings?.ContainerDesign.GetInvariant(name);

    public override string? Classes(string tag)
    {
        if (GetSettings(tag) is not MagicContainerDesignSettingsItem styles) return null;

        var value = GetClasses(styles);
        return PostProcess(value);
        //var tokens = Settings.Tokens.Expanded(new ModuleTokens(_module));
        //return tokens.Parse(value).EmptyAsNull();
    }

    protected override TokenEngine Tokens => _tokens1 ??= Settings.Tokens.Expanded(new ModuleTokens(_module));
    private TokenEngine? _tokens1;


    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    private string GetClasses(MagicContainerDesignSettingsItem styles)
    {
        var value =  string.Join(" ", new[]
        {
            styles.Classes,
            styles.IsPublished.Get(_module.IsPublished()),      // Info-Class if not published
            styles.IsAdminModule.Get(_module.UseAdminContainer)       // Info-class if admin module
        }.Where(s => s.HasValue()));

        return value;
    }
}