using System.Text.Json.Serialization;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

internal interface IInherit
{
    /// <summary>
    /// Determines if it inherits another property
    /// </summary>
    [JsonPropertyName("@inherits")]
    string? Inherits { get; set; }
}