namespace ToSic.Cre8magic.Client.Breadcrumbs.Settings
{
    public interface IBreadcrumbDesigner
    {
        string Classes(string tag, MagicBreadcrumbItem item);
        string Value(string key, MagicBreadcrumbItem item);
    }
}
