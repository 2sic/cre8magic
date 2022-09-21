using System.Text.Json.Serialization;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public abstract class SettingsWithInherit: IInherit
{
    internal const string InheritsNameInJson = "@inherits";
    [JsonPropertyName(InheritsNameInJson)]
    public string? Inherits { get; set; }
}