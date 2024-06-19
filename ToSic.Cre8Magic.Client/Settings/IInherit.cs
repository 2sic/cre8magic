using System.Text.Json.Serialization;

namespace ToSic.Cre8magic.Client.Settings;

internal interface IInherit
{
    /// <summary>
    /// Determines if it inherits another property
    /// </summary>
    [JsonPropertyName(SettingsWithInherit.InheritsNameInJson)]
    string? Inherits { get; set; }
}