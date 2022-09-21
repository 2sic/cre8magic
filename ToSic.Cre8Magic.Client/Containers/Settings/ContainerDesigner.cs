using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

internal class ContainerDesigner 
{
    internal const string ModulePrefixDefault = "module";

    public ContainerDesigner(MagicSettings settings, Module module)
    {
        _settings = settings;
        _module = module;
    }

    private readonly MagicSettings _settings;
    private readonly Module _module;


    internal string? Classes(string tag)
    {
        var styles = _settings.ContainerDesign.FindInvariant(tag); // safe, also does null-check
        if (styles is null) return null;

        var value = /*new ContainerDesigner(settings, module).*/GetClasses(styles);
        var tokens = _settings.Tokens.Expanded(new ModuleTokens(_module));
        return tokens.Parse(value);
    }



    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    public string GetClasses(MagicContainerDesignSettingsItem styles)
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