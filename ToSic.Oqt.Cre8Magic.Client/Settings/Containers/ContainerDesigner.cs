using Oqtane.Models;

namespace ToSic.Oqt.Cre8Magic.Client.Settings.Containers;

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
            _module.IsPublished() ? styles.IsPublished : styles.IsNotPublished, // Info-Class if not published
            _module.UseAdminContainer ? styles.IsAdminModule : styles.IsNotAdminModule // Info-class if admin module
        }.Where(s => s.HasValue()));

        return value;
    }
}