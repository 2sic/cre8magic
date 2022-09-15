﻿namespace ToSic.Oqt.Cre8Magic.Client.Settings.Themes;

/// <summary>
/// Constants and helpers related to creating Css and Css Classes.
///
/// If you change these, you must also update the SCSS files. 
/// </summary>
public class MagicThemeDesignSettings
{
    private const string SitePrefixDefault = "site";
    private const string PagePrefixDefault = "page";
    internal const string MainPrefix = "to-shine";
    private const string PanePrefixDefault = "pane";
    private const string MenuLevelPrefixDefault = "nav-level";
    private const string BodyDivId = "cre8magic-root";

    public const string SettingFromDefaults = $"{MainPrefix}-warning-this-is-from-defaults-you-should-set-your-own-value";

    public string[] MagicContext { get; set; } =
    {
        //1.2 Set the page-### class
        $"{PagePrefixDefault}-{MagicTokens.PageId}",
        //1.4 Set the page-root-### class
        $"{PagePrefixDefault}-root-{MagicTokens.PageRootId}",
        //1.3 Set the page-parent-### class
        $"{PagePrefixDefault}-parent-{MagicTokens.PageParentId}",
        //2 Set site-### class
        $"{SitePrefixDefault}-{MagicTokens.SiteId}",
        //3 Set the nav-level-### class
        $"{MenuLevelPrefixDefault}-{MagicTokens.MenuLevel}",
        //5.1 Set the to-shine-variation- class
        $"{MainPrefix}-variation-{MagicTokens.LayoutVariation}",

        //5.2 Set the to-shine-mainnav-variation- class to align the menu
        $"{MainPrefix}-mainnav-variation-right",

        // Debug info so we know the defaults were used
        SettingFromDefaults
    };

    public string PageIsHome { get; set; } = $"{PagePrefixDefault}-is-home";


    public string PaneIsEmpty { get; set; } = $"{PanePrefixDefault}-is-empty";

    public string MagicContextTagId { get; set; } = BodyDivId;

    // TODO:
    public NamedSettings<string> Other { get; set; } = new();

    // TODO: initialize with real properties, so the defaults don't already contain something?
    public static MagicThemeDesignSettings Defaults = new();
}