using Oqtane.Models;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

public class MagicContainerSettings: SettingsWithInherit
{
    public NamedSettings<DesignSettingBase> Custom { get; set; } = new();

    private const string IdKey = "Id";
    private const string IdDefault = "module-[Module.Id]";

    internal string? Value(MagicSettings settings, Module module, string key)
    {
        var value = Custom.FindInvariant(key); // safe, also does null-check
        if (value == null || !value.Value.HasValue()) return null;

        var tokens = settings.Tokens.Expanded(new ModuleTokens(module));
        return tokens.Parse(value.Value);
    }

    private static readonly MagicContainerSettings FbAndF = new()
    {
        Custom = new()
        {
            { IdKey, new() { Value = IdDefault } }
        },
    };

    internal static Defaults<MagicContainerSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };

}