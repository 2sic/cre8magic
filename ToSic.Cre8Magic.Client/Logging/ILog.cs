namespace ToSic.Cre8magic.Client.Logging;

internal interface ILog
{
    LogRoot LogRoot { get; }

    string Prefix { get; }

    int Depth { get; set; }
}