namespace ToSic.Cre8magic.Client.Analytics;

public class MagicAnalyticsSettings : SettingsWithInherit
{
    // public NamedSettings<DesignSetting> Custom { get; set; } = new();

    public string? GtmId { get; set; }

    public bool? PageViewTrack { get; set; }

    public bool? PageViewTrackFirst { get; set; }

    public string? PageViewJs { get; set; }

    public string? PageViewEvent { get; set; }


    private static readonly MagicAnalyticsSettings FbAndF = new()
    {
        GtmId = null,
        PageViewTrack = false,
        PageViewTrackFirst = false,
        PageViewJs = "gtag",
        PageViewEvent = "blazor_page_view"
    };

    internal static Defaults<MagicAnalyticsSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };

}