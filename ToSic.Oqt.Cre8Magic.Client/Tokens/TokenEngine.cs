using System.Collections.ObjectModel;

namespace ToSic.Oqt.Cre8Magic.Client.Tokens;

internal class TokenEngine: ITokenReplace
{
    public const string NameIdConst = nameof(TokenEngine);

    public TokenEngine() => Parsers = new List<ITokenReplace>().AsReadOnly();

    public TokenEngine(List<ITokenReplace> parsers) => Parsers = parsers.AsReadOnly();

    public ReadOnlyCollection<ITokenReplace> Parsers { get; }

    public TokenEngine Expanded(ITokenReplace add) => new(Parsers.Concat(new List<ITokenReplace> { add }).ToList());

    public TokenEngine Replaced(string nameId, ITokenReplace replacement)
    {
        // Create new list preserving the initial order
        var newParsers = Parsers.Select(p => p.NameId == nameId ? replacement : p).ToList();

        // Determine if we replaced it, otherwise append
        if (newParsers.All(p => p.NameId != nameId))
            newParsers.Add(replacement);
        return new(newParsers);
    }

    public string NameId => NameIdConst;

    public string Parse(string value)
    {
        if (!value.HasValue() || !value.Contains(MagicPlaceholders.PlaceholderMarker)) return value;
        foreach (var p in Parsers)
        {
            value = p.Parse(value);
            if (!value.Contains(MagicPlaceholders.PlaceholderMarker))
                return value;
        }

        return value;
    }
}