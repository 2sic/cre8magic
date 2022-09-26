using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Logging;

internal class Log: ILog
{
    [JsonIgnore]
    public readonly List<LogEntry> LogEntries = new();

    public IEnumerable<string> Entries => LogEntries.Select(e => e.ToString());
    
    Log ILog.Log => this;
}