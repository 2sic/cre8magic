namespace ToSic.Oqt.Cre8Magic.Client.Tokens;

public class ThemeTokens: ITokenReplace
{
    public const string NameIdConstant = nameof(ThemeTokens);

    public ThemeTokens(MagicPackageSettings themeSettings)
    {
        PackageSettings = themeSettings;

    }

    internal MagicPackageSettings PackageSettings { get; }

    public string NameId => NameIdConstant;

    public virtual string Parse(string value)
    {
        return value
            .Replace(MagicPlaceholders.ThemeAssetsPath, PackageSettings.AssetsPath)
            .Replace(MagicPlaceholders.ThemePath, PackageSettings.Path);
    }
}