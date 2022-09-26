namespace ToSic.Cre8Magic.Client.Logging;

internal class LogChild: ILog
{
    public Log Log { get; }
    public string Prefix { get; }

    internal LogChild(Log log, string prefix)
    {
        Log = log;
        Prefix = prefix;
    }
}