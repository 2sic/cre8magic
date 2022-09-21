namespace ToSic.Cre8Magic.Client.Settings;

internal interface IHasDebugSettings
{
    MagicDebugSettings? Debug { get; }
}