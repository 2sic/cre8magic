using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Logging;

internal class LogRoot
{
    [JsonIgnore]
    public readonly List<LogEntry> LogEntries = new();

    public IEnumerable<object?> Entries => LogEntries.SelectMany(e =>
    {
        if (e?.Data == null) return new [] { e?.ToString() as object};
        return new[] { e?.ToString(), new { e.Data } as object };
    });

    [JsonIgnore]
    public int Depth { get; set; } = 0;

    public Log GetLog(string? prefix) => new(this, Depth + 1, prefix);
}