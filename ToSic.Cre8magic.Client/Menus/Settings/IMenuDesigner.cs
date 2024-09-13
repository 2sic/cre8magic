namespace ToSic.Cre8magic.Client.Menus.Settings;

public interface IMenuDesigner
{
    string Classes(string tag, MagicMenuPage page);
    string Value(string key, MagicMenuPage page);
}