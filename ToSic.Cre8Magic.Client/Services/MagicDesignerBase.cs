namespace ToSic.Cre8Magic.Client.Services;

internal abstract class MagicDesignerBase: MagicServiceWithSettingsBase
{
    protected abstract DesignSettingBase? GetSettings(string name);

    protected virtual TokenEngine Tokens => _tokens ??= Settings.Tokens;
    private TokenEngine? _tokens;

    protected virtual bool ParseTokens => true;

    protected string? PostProcess(string? value)
    {
        if (!ParseTokens) return value.EmptyAsNull();
        return Tokens.Parse(value).EmptyAsNull();
    }

    public virtual string? Classes(string target) => PostProcess(GetSettings(target)?.Classes);

    public string? Value(string target) => PostProcess(GetSettings(target)?.Value);

    public string? Id(string name) => PostProcess(GetSettings(name)?.Id);

}