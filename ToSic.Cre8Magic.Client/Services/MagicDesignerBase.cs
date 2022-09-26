namespace ToSic.Cre8Magic.Client.Services;

internal abstract class MagicDesignerBase: MagicServiceWithSettingsBase
{
    protected abstract DesignSettingBase? GetSettings(string name);

    public virtual string? Classes(string target) => GetSettings(target)?.Classes.EmptyAsNull();

    public string? Value(string target) => GetSettings(target)?.Value.EmptyAsNull();

    public string? Id(string name) => GetSettings(name)?.Id.EmptyAsNull();

}