using System.Text.Json.Serialization;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public abstract class SettingsWithInherit: IInherit
{
    [JsonPropertyName("@inherits")]
    public string? Inherits { get; set; }
}