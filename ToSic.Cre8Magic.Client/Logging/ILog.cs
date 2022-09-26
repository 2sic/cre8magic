namespace ToSic.Cre8Magic.Client.Logging;

internal interface ILog
{
    LogRoot LogRoot { get; }

    string Prefix { get; }

    int Depth { get; set; }
}