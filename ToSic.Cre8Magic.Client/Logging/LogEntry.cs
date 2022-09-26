namespace ToSic.Cre8Magic.Client.Logging;

internal class LogEntry
{

    public LogEntry(ILog? log, string message, int depth, CodeRef codeRef)
    {
        Depth = depth;
        Source = log?.Prefix ?? "";
        Log = log;
        CodeRef = codeRef;
        Message = message;
    }

    public string Source { get; }

    public string Message { get; }

    public ILog? Log { get; }
    public CodeRef CodeRef { get; }

    public string? Result { get; private set; }

    public int Depth;

    public void AppendResult(string message)
    {
        Result = message;
        //WrapOpenWasClosed = true;
    }


    public override string ToString()
    {
        return $"{Source}{(Source.HasValue() ? ": " : "")}" +
               new string('>', Math.Max(0, Depth - 1)) +
               $"{Message}" +
               $"{(Result.HasValue() ? $"='{Result}'" : "")}";
    }
}