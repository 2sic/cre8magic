namespace ToSic.Oqt.Cre8Magic.Client.Menus.Settings;
using static MagicTokens;

public partial class MagicMenuDesignSettings
{

    internal static Defaults<MagicMenuDesignSettings> Defaults = new()
    {
        // The default/fallback design configuration for menus.
        // Normally this would be set in the json file or the theme settings, so this wouldn't be used. 
        Fallback = new()
        {
            {
                "a", new()
                {
                    IsActive = new("active"),
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
                    IsActive = new("active"),
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
        },
    };
}