using System.Text.Json.Serialization;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

[JsonConverter(typeof(PairOnOffJsonConverter))]
public class PairOnOff
{
    /// <summary>
    /// Empty constructor for JSON serialization
    /// </summary>
    public PairOnOff() {}

    public PairOnOff(string? on, string? off = null)
    {
        On = on;
        Off = off;
    }

    public string? On { get; set; }
    public string? Off { get; set; }
}

public static class PairOnOffExtensions
{
    /// <summary>
    /// Null-safe pair access
    /// </summary>
    /// <param name="pair"></param>
    /// <param name="isOn"></param>
    /// <returns></returns>
    public static string? Get(this PairOnOff? pair, bool? isOn) => isOn == true ? pair?.On : pair?.Off;
}