namespace ToSic.Cre8Magic.Client.Containers.Settings;

public class MagicContainerSettings: SettingsWithInherit
{
    public NamedSettings<DesignSettingBase> Custom { get; set; } = new();

    private static readonly MagicContainerSettings FbAndF = new()
    {
        Custom = new()
    };

    internal static Defaults<MagicContainerSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };

}