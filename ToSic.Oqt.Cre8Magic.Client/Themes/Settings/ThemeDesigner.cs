﻿using Oqtane.UI;

namespace ToSic.Oqt.Cre8Magic.Client.Themes.Settings;

/// <summary>
/// Special helper to figure out what classes should be applied to the page. 
/// </summary>
internal class ThemeDesigner : MagicServiceWithSettingsBase
{
    internal string BodyClasses(PageState pageState, ITokenReplace tokens)
    {
        var css = Settings?.Page;

        if (css == null) throw new ArgumentException("Can't continue without CSS specs", nameof(css));

        // Make a copy...
        var classes = css.MagicContext.ToList();
        if (pageState.Page.Path == "") classes.Add(css.PageIsHome);

        // Do these once multi-language is better
        //1.5 Set the page-root-neutral-### class
        // do once Multilanguage is good


        //4.1 Set lang- class
        // do once lang is clear
        //4.2 Set the lang-root- class
        // do once lang is clear
        //4.3 Set the lang-neutral- class
        // do once lang is clear

        var bodyClasses = string.Join(" ", classes).Replace("  ", " ");

        return tokens.Parse(bodyClasses);
    }


    public bool PaneIsEmpty(PageState pageState, string paneName)
    {
        var paneHasModules = pageState.Modules.Any(
            module => !module.IsDeleted
                      && module.PageId == pageState.Page.PageId
                      && module.Pane == paneName);

        return !paneHasModules;
    }

    public string PaneIsEmptyClasses(PageState pageState, string paneName)
        => PaneIsEmpty(pageState, paneName) ? Settings?.Page.PaneIsEmpty ?? "" : "";

    public string? Classes(string target) => Settings?.Page.Classes.GetInvariant(target).EmptyAsNull();
}