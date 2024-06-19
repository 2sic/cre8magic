namespace ToSic.Cre8magic.Client.Settings;

public interface IHasSettingsExceptions
{
    public bool HasExceptions => Exceptions?.Any() ?? false;

    List<Exception> Exceptions { get; }
}