namespace ToSic.Cre8Magic.Client.Settings;

/// <summary>
/// Anything that can define what classes it should have.
///
/// This is usually the base class for something that can also have more information.
/// </summary>
public class DesignSettingActive: DesignSettingBase
{
    /// <summary>
    /// Classes to apply if this thing is active.
    /// For example, the current page or language. 
    /// </summary>
    public PairOnOff? IsActive { get; set; }

}