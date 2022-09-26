using System.Runtime.CompilerServices;

namespace ToSic.Cre8Magic.Client.Logging;

internal static class LogExtensions
{
    /// <summary>
    /// add an existing entry of another logger
    /// </summary>
    /// <param name="log"></param>
    /// <param name="entry"></param>
    internal static void Add(this ILog log, LogEntry entry)
    {
        // prevent parallel threads from updating entries at the same time
        /*lock (log.Entries) { */log.Log.LogEntries.Add(entry); /*}*/
    }

    /// <summary>
    /// Add a message
    /// </summary>
    internal static void AddInternal(this ILog? log, string message, CodeRef code)
    {
        // Null-check
        // if (!(log is Log realLog)) return;
        if (log == null) return;
        var e = new LogEntry((log as LogChild)?.Prefix ?? "", /*log,*/ message, /*realLog.WrapDepth,*/ code);
        log.Add(e);
    }

    public static void A(this ILog? log,
        string message,
        [CallerFilePath] string cPath = null,
        [CallerMemberName] string cName = null
        //[CallerLineNumber] int cLine = 0
    ) => log?.AddInternal(message, new CodeRef(cPath, cName/*, cLine*/));

    public static void Call(this ILog? log,
        string message,
        [CallerFilePath] string cPath = null,
        [CallerMemberName] string cName = null
        //[CallerLineNumber] int cLine = 0
    ) => log?.AddInternal($"{cName}({message})", new CodeRef(cPath, cName/*, cLine*/));

}