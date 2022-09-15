﻿using ToSic.Oqt.Cre8Magic.Client.Breadcrumbs.Settings;
using static ToSic.Oqt.Cre8Magic.Client.MagicConstants;

namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public partial class MagicPackageSettings
{
    internal static MagicPackageSettings Fallback = new()
    {
        Defaults = new()
        {
            Source = "Preset",
            Containers = new()
            {
                { Default, MagicContainerSettings.Defaults }
            },
            ContainerDesigns = new()
            {
                { Default, MagicContainerDesignSettings.Defaults }
            },

            Layouts = new()
            {
                { Default, MagicThemeSettings.Defaults }
            },

            Languages = new()
            {
                { Default, MagicLanguagesSettings.Defaults }
            },
            LanguageDesigns = new()
            {
                { Default, MagicLanguageDesignSettings.Defaults }
            },
            Breadcrumbs = new()
            {
                { Default, MagicBreadcrumbSettings.Defaults }
            },
            Menus = new()
            {
                { Default, MagicMenuSettings.Defaults },
            },
            MenuDesigns = new()
            {
                // The Default design, if not overridden in the JSON
                { Default, MagicMenuDesignSettings.Defaults },
                // The Design configuration for Mobile menus, if not overridden by the JSON
                { DesignMobile, MagicMenuDesignSettings.MobileDefaults }
            },
            PageDesigns = new()
            {
                { Default, MagicThemeDesignSettings.Defaults }
            }
        },
    };
}