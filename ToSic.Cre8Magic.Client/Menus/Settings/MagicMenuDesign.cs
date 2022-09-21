namespace ToSic.Cre8Magic.Client.Menus.Settings;

public class MagicMenuDesign: DesignSettingActive
{
    /// <summary>
    /// List of classes to add on certain levels only.
    /// Use level -1 to specify classes to apply to all the remaining ones which are not explicitly listed.
    /// </summary>
    public Dictionary<int, string>? ByLevel { get; set; }

    /// <summary>
    /// Classes to add if this node is a parent (has-children).
    /// </summary>
    public PairOnOff? HasChildren { get; set; }

    /// <summary>
    /// Classes to add if the node is disabled.
    /// TODO: unclear why it's disabled, what would cause this...
    /// </summary>
    public PairOnOff? IsDisabled { get; set; }

    /// <summary>
    /// Classes to add if this node is in the path / breadcrumb of the current page.
    /// </summary>
    public PairOnOff? InBreadcrumb { get; set; }


    /// <summary>
    /// Special key to get a value - for non-css configurations
    /// </summary>
    public string? Value { get; set; }
}