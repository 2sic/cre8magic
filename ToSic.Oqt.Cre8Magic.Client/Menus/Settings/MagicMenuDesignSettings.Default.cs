﻿namespace ToSic.Oqt.Cre8Magic.Client.Menus.Settings;
using static MagicTokens;

public partial class MagicMenuDesignSettings
{
    /// <summary>
    /// The default/fallback design configuration for menus.
    /// Normally this would be set in the json file or the theme settings, so this wouldn't be used. 
    /// </summary>
    public static MagicMenuDesignSettings Defaults => new()
    {
        {
            "a", new()
            {
                IsActive = "active",
                HasChildren = "dropdown-toggle",
                ByLevel = new()
                {
                    { ByLevelOtherKey, "dropdown-item" },
                    { 1, "nav-link" },

                }
            }
        },
        {
            "li", new()
            {
                Classes = $"nav-item nav-{PageId}",
                HasChildren = "has-child dropdown",
                IsActive = "active",
                IsDisabled = "disabled",
            }

        },
        {
            "ul", new()
            {
                ByLevel = new()
                {
                    { ByLevelOtherKey, "dropdown-menu" },
                    { 0, "navbar-nav" },
                },
            }
        }
    };
}