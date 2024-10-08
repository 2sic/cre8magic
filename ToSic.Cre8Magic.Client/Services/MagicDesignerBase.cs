﻿namespace ToSic.Cre8magic.Client.Services;

internal abstract class MagicDesignerBase: MagicServiceWithSettingsBase, IMagicDesigner
{
    protected internal MagicDesignerBase() {}

    protected internal MagicDesignerBase(MagicSettings settings) => InitSettings(settings);

    protected abstract DesignSetting? GetSettings(string name);

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