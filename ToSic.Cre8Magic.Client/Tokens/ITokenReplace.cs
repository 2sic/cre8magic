namespace ToSic.Cre8magic.Client.Tokens;

internal interface ITokenReplace
{
    string NameId { get; }
    string? Parse(string? value);
}