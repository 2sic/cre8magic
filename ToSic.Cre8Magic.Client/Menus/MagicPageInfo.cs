namespace ToSic.Cre8Magic.Client.Menus;

internal class MagicPageInfo
{
    public bool IsActive;
    public bool HasChildren;
    public bool InBreadcrumb;

    public string Log => $"{nameof(IsActive)}={IsActive}, {nameof(InBreadcrumb)}={InBreadcrumb}, {nameof(HasChildren)}={HasChildren}";
}