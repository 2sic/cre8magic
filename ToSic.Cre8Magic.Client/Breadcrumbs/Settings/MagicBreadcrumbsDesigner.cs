namespace ToSic.Cre8Magic.Client.Breadcrumbs.Settings;

internal class MagicBreadcrumbsDesigner: MagicDesignerBase
{
    public MagicBreadcrumbsDesigner(MagicSettings settings)
    {
        InitSettings(settings);
    }

    protected override DesignSettingBase? GetSettings(string name)
    {
        return Settings?.BreadcrumbsDesigns.GetInvariant(name);
    }
}