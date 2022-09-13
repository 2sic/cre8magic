namespace ToSic.Oqt.Cre8Magic.Client.Tokens;

internal class TokenEngine: ITokenReplace
{
    public const string NameIdConst = nameof(TokenEngine);

    public TokenEngine() => Parsers = new();

    public TokenEngine(List<ITokenReplace> parsers) => Parsers = parsers;

    public List<ITokenReplace> Parsers { get; }

    public TokenEngine Expanded(ITokenReplace add) => new(Parsers.Concat(new List<ITokenReplace> { add }).ToList());

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