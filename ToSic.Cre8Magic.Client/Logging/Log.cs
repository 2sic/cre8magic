namespace ToSic.Cre8Magic.Client.Logging;

internal class Log: ILog
{
    public LogRoot LogRoot { get; }
    public string Prefix { get; }

    internal Log(LogRoot logRoot, int depth, string prefix)
    {
        LogRoot = logRoot;
        Prefix = prefix;
        Depth = depth;
    }

    public int Depth { get; set; }

    //public Log GetLog(string? prefix) => new(LogRoot, Depth + 1, prefix);
}