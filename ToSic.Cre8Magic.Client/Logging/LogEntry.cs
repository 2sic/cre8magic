namespace ToSic.Cre8Magic.Client.Logging;

internal class LogEntry
{

    public LogEntry(string source, string message, CodeRef codeRef)
    {
        Source = source;
        CodeRef = codeRef;
        Message = message;
    }

    public string Source { get; }

    public string Message { get; }

    public CodeRef CodeRef { get; }

    public override string ToString() => $"{Source}{(Source.HasValue() ? ": " : "")}{Message}";
}