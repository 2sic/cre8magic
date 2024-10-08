﻿using static ToSic.Cre8magic.Client.MagicTokens;

namespace ToSic.Cre8magic.Client.Settings;

internal class DefaultSettings
{
    internal static Defaults<NamedSettings<MagicMenuDesign>> Defaults = new()
    {
        // The default/fallback design configuration for menus.
        // Normally this would be set in the json file or the theme settings, so this wouldn't be used. 
        Fallback = new()
        {
            {
                "a", new()
                {
                    IsActive = new("active"),
                    HasChildren = new("dropdown-toggle"),
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
                    HasChildren = new("has-child dropdown"),
                    IsActive = new("active"),
                    IsDisabled = new("disabled"),
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
        },
    };
}