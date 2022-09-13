using Oqtane.Models;

namespace ToSic.Oqt.Cre8Magic.Client.Styling;

internal class ModuleStyler 
{

    public ModuleStyler(Module module) => _module = module;
    private readonly Module _module;

        

    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    public string GetClasses(MagicContainerDesign styles)
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