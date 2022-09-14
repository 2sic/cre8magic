using System.Collections.ObjectModel;

namespace ToSic.Oqt.Cre8Magic.Client.Tokens;

/// <summary>
/// Basic token engine which takes a list of token replacers and runs them.
/// In future, should be a bit more modern using a token parser and token providers
/// similar to 2sxc.
/// But ATM there are only ca. 10 tokens so the current model is probably sufficient
/// </summary>
internal class TokenEngine: ITokenReplace
{
    public const string NameIdConst = nameof(TokenEngine);
    public string NameId => NameIdConst;

    #region Constructors and Child-Makers

    public TokenEngine() => Parsers = new List<ITokenReplace>().AsReadOnly();

    public TokenEngine(List<ITokenReplace> parsers) => Parsers = parsers.AsReadOnly();

    public ReadOnlyCollection<ITokenReplace> Parsers { get; }

    public TokenEngine Expanded(ITokenReplace add) => new(Parsers.Concat(new List<ITokenReplace> { add }).ToList());

    #endregion

    public TokenEngine SwapParser(ITokenReplace replacement)
    {
        // Create new list preserving the initial order
        var newParsers = Parsers
            .Select(p => p.NameId == replacement.NameId ? replacement : p)
            .ToList();

        // Determine if we replaced it, otherwise append
        if (newParsers.All(p => p.NameId != replacement.NameId))
            newParsers.Add(replacement);
        return new(newParsers);
    }


    public string Parse(string value)
    {
        if (!value.HasValue() || !value.Contains(MagicTokens.PlaceholderMarker)) return value;
        foreach (var p in Parsers)
        {
            value = p.Parse(value);
            if (!value.Contains(MagicTokens.PlaceholderMarker))
                return value;
        }

        return value;
    }
}