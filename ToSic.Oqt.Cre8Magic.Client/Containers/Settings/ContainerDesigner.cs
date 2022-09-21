using Oqtane.Models;

namespace ToSic.Oqt.Cre8Magic.Client.Containers.Settings;

internal class ContainerDesigner 
{

    public ContainerDesigner(Module module) => _module = module;
    private readonly Module _module;

        

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