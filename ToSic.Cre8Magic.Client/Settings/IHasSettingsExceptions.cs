namespace ToSic.Cre8Magic.Client.Settings;

public interface IHasSettingsExceptions
{
    public bool HasExceptions => Exceptions?.Any() ?? false;

    List<Exception> Exceptions { get; }
}