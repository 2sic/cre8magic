namespace ToSic.Cre8Magic.Client.Settings;

internal class SettingsDefaultsAttribute : Attribute
{
    public SettingsDefaultsAttribute(object defaults, object merge, Func<object> getDefaults)
    {
        Defaults = defaults;
        Merge = merge;
    }

    public object Defaults { get; set; }

    public object Merge { get; set; }
}