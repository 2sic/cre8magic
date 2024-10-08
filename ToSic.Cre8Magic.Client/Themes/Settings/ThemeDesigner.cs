﻿namespace ToSic.Cre8magic.Client.Themes.Settings;

/// <summary>
/// Special helper to figure out what classes should be applied to the page. 
/// </summary>
internal class ThemeDesigner : MagicDesignerBase
{
    public ThemeDesigner(MagicSettings settings) : base(settings) {}

    internal string? BodyClasses(ITokenReplace tokens)
    {
        var css = Settings?.ThemeDesign;

        if (css == null) throw new ArgumentException("Can't continue without CSS specs", nameof(css));

        // Make a copy...
        var classes = css.MagicContext.ToList();
        classes.Add(css.PageIsHome?.Get(Settings.PageState.CurrentPageIsHome()));

        // Do these once multi-language is better
        //1.5 Set the page-root-neutral-### class
        // do once Multilanguage is good


        //4.1 Set lang- class
        // do once lang is clear
        //4.2 Set the lang-root- class
        // do once lang is clear
        //4.3 Set the lang-neutral- class
        // do once lang is clear

        var bodyClasses = string.Join(" ", classes.Where(c => c.HasValue())).Replace("  ", " ");

        return tokens.Parse(bodyClasses);
    }


    private bool PaneIsEmpty(string paneName)
    {
        if (Settings == null) return true;
        var pageState = Settings.PageState;
        var paneHasModules = pageState.Modules.Any(
            module => !module.IsDeleted
                      && module.PageId == pageState.Page.PageId
                      && module.Pane == paneName);

        return !paneHasModules;
    }

    public string? PaneClasses(string paneName) => Settings?.ThemeDesign.PaneIsEmpty.Get(PaneIsEmpty(paneName));

    protected override DesignSetting? GetSettings(string name) => Settings?.ThemeDesign.Custom.GetInvariant(name);
}