using System.Runtime.CompilerServices;

namespace ToSic.Cre8Magic.Client.Logging;

internal static class LogExtensions
{
    /// <summary>
    /// add an existing entry of another logger
    /// </summary>
    /// <param name="log"></param>
    /// <param name="entry"></param>
    internal static void AddInternal(this ILog log, LogEntry entry)
    {
        // prevent parallel threads from updating entries at the same time
        /*lock (log.Entries) { */log.LogRoot.LogEntries.Add(entry); /*}*/
    }

    /// <summary>
    /// Add a message
    /// </summary>
    internal static void AddInternal(this ILog? log, string message, CodeRef code)
    {
        // Null-check
        // if (!(log is Log realLog)) return;
        if (log == null) return;
        var e = new LogEntry(log, message, log.Depth, code);
        log.AddInternal(e);
    }

    internal static LogEntry AddInternalReuse(this ILog? log, string message, CodeRef code)
    {
        // Null-check
        if (!(log != null)) return new(null, null, 0, code);
        var e = new LogEntry(log, message, log.Depth, code);
        log.AddInternal(e);
        return e;
    }

    public static void A(this ILog? log,
        string message,
        [CallerFilePath] string cPath = null,
        [CallerMemberName] string cName = null,
        [CallerLineNumber] int cLine = 0
    ) => log?.AddInternal(message, new(cPath, cName, cLine));

    public static LogCall Fn(this ILog log, string parameters = null, string message = null, bool startTimer = false,
        [CallerFilePath] string cPath = null, [CallerMemberName] string cName = null,
        [CallerLineNumber] int cLine = 0) =>
        new(log, new(cPath, cName, cLine), false, parameters, message, startTimer);

    public static LogCall<T> Fn<T>(this ILog? log,
        string parameters = null,
        string message = null,
        bool startTimer = false,
        CodeRef? code = null,
        [CallerFilePath] string cPath = null,
        [CallerMemberName] string cName = null,
        [CallerLineNumber] int cLine = 0
    ) => new(log, code ?? new CodeRef(cPath, cName, cLine), false, parameters, message, startTimer);


    public static void Call(this ILog? log,
        string message,
        [CallerFilePath] string cPath = null,
        [CallerMemberName] string cName = null,
        [CallerLineNumber] int cLine = 0
    ) => log?.AddInternal($"{cName}({message})", new(cPath, cName, cLine));

}