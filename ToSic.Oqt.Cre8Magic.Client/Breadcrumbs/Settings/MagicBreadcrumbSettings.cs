using Oqtane.Themes.BlazorTheme;

namespace ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;

public class MagicBreadcrumbSettings
{
    public string? Separator { get; set; }
    private const string BreadcrumbSeparatorDefault = "&nbsp;&rsaquo;&nbsp;";

    public string? Revealer { get; set; }

    private const string BreadcrumbRevealDefault = "…"; // Ellipsis character

    private static readonly MagicBreadcrumbSettings FbAndF = new()
    {
        Separator = BreadcrumbSeparatorDefault,
        Revealer = BreadcrumbRevealDefault,
    };

    internal static Defaults<MagicBreadcrumbSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };
}