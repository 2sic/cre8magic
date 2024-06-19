namespace ToSic.Cre8magic.Client.Services;

public abstract class MagicServiceWithSettingsBase
{
    public void InitSettings(MagicSettings settings) => Settings = settings;

    public MagicSettings? Settings { get; private set; }

}