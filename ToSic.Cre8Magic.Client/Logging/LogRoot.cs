using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Logging;

internal class LogRoot // : ILog
{
    [JsonIgnore]
    public readonly List<LogEntry> LogEntries = new();

    public IEnumerable<string> Entries => LogEntries.Select(e => e.ToString());

    //[JsonIgnore]
    //LogStack ILog.Log => this;

    [JsonIgnore]
    public int Depth { get; set; } = 0;

    public Log GetLog(string? prefix) => new(this, Depth + 1, prefix);
}