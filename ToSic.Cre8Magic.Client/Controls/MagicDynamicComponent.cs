namespace ToSic.Cre8Magic.Client.Controls;

public class MagicDynamicComponent
{
    public MagicDynamicComponent(string group, Type type, Dictionary<string, object>? parameters)
    {
        Group = group;
        Type = type;
        Parameters = parameters;
    }

    public string Group { get; set; }

    public Type Type { get; set; }

    public Dictionary<string, object>? Parameters { get; set; }
}