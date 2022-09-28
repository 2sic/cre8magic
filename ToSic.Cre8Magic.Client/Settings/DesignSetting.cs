namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// Anything that can define what classes it should have.
///
/// This is usually the base class for something that can also have more information.
/// </summary>
public class DesignSetting
{
    /// <summary>
    /// Classes which are applied to all the tags of this type
    /// </summary>
    public string? Classes { get; set; }

    /// <summary>
    /// Special key to get a value - for non-css configurations
    /// </summary>
    public string? Value { get; set; }


    public string? Id { get; set; }

    /// <summary>
    /// Classes to apply if this thing is active.
    /// For example, the current page or language. 
    /// </summary>
    public PairOnOff? IsActive { get; set; }

    /// <summary>
    /// If something is published or not, usually just for Containers
    /// </summary>
    public PairOnOff? IsPublished { get; set; }

    /// <summary>
    /// If a module is admin or not, usually just for containers
    /// </summary>
    public PairOnOff? IsAdmin { get; set; }

}