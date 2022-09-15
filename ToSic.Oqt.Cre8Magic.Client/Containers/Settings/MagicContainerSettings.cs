using Oqtane.Models;

namespace ToSic.Oqt.Cre8Magic.Client.Containers.Settings;

public class MagicContainerSettings
{
    public NamedSettings<string> Values { get; set; } = new();

    private const string IdKey = "Id";
    private const string IdDefault = "module-[Module.Id]";

    internal string? Value(MagicSettings settings, Module module, string key)
    {
        var value = Values.FindInvariant(key); // safe, also does null-check
        if (!value.HasValue()) return null;

        var tokens = settings.Tokens.Expanded(new ModuleTokens(module));
        return tokens.Parse(value!);
    }

    private static readonly MagicContainerSettings FbAndF = new()
    {
        Values = new()
        {
            { IdKey, IdDefault }
        },
    };

    internal static Defaults<MagicContainerSettings> Defaults = new()
    {
        Fallback = FbAndF,
        Foundation = FbAndF,
    };

}