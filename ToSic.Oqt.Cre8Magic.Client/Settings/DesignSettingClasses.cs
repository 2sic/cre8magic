﻿namespace ToSic.Oqt.Cre8Magic.Client.Settings;

/// <summary>
/// Anything that can define what classes it should have.
///
/// This is usually the base class for something that can also have more information.
/// </summary>
public class DesignSettingClasses
{
    /// <summary>
    /// Classes which are applied to all the tags of this type
    /// </summary>
    public string? Classes { get; set; }

}