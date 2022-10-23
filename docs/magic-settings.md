---
title: Magic Settings
permalink: /magic-settings
icon: fa-cog
order: 20
---

# cre8magic – Magic Settings

**Magic Settings** allow you to move 95% of the theme code into some kind of configuration.

## Overview

Basically the magic settings let you put a bunch of parameters in a JSON file.
This file is then used by your Theme and it's Controls to

* determine what Blazor files to use (like what Template should be used for the menu)
* what to do with `class="..."` or `id="..."` in the HTML
* and much more 😉

> This basic principle allows you to create and tweak amazing designs
> without ever recompiling the Theme.
>
> It also allows you to create variations of your theme with the same Blazor files.

## Example JSON

See [settings-json](./theme-json.md) to see an example file.

## The Configuration File

The system works by creating a json file such as `theme-settings.json`.

This is placed in your themes `wwwroot` folder like this:

`wwwroot/ToSic.Themes.BlazorCms/theme-settings.json`

_Note that we don't use `...Client` in the path, just the real namespace._

Which file to use can be configured in the theme.
Normally you would use the same file for all variations of your theme, but the important thing is that the theme
must give some initial configuration object to the cre8magic Services.

Here's how:

### Create the MagicPackageSettings

This could be done anywhere, but I would place the code in the `ThemeInfo.cs` file:

```c#
    /// <summary>
    /// Default settings used in this package.
    /// They are defined here and given as initial values to the ThemeSettingsService in the Default Razor file.
    /// </summary>
    public static MagicPackageSettings ThemePackageDefaults = new()
    {
        // The package name is important, as it's used to find assets etc.
        PackageName = new ThemeInfo().Theme.PackageName,

        // The json file in the theme folder folder containing all kinds of settings etc.
        SettingsJsonFile = "theme-settings.json",
    };
```

### Tell the Theme to Use these Settings

Then in the theme, you should inherit from the `MagicTheme` base class and set the ThemePackageSettings like this:

```c#
public override MagicPackageSettings ThemePackageSettings => ThemeInfo.ThemePackageDefaults;
```

This would usually look a bit like this:

```c#
public abstract class MyThemeBase : MagicTheme
{
    public override List<Resource> Resources => new()
    {
        new() { ResourceType = Stylesheet, Url = $"{ThemePath()}theme.min.css" },       // Bootstrap generated with Sass/Webpack
        new() { ResourceType = Script, Url = $"{ThemePath()}bootstrap.bundle.min.js" }, // Bootstrap JS
        new() { ResourceType = Script, Url = $"{ThemePath()}ambient.js", },             // Ambient JS for page Up-button etc.
    };

    /// <summary>
    /// The ThemePackageSettings must be set in this class, so the Settings initializer can pick it up.
    /// </summary>
    public override MagicPackageSettings ThemePackageSettings => ThemeInfo.ThemePackageDefaults;

    public override string Panes => string.Join(",", PaneNames.Default, PaneNameHeader);
}
```

that's it ✌🏽

## How the Settings Work

Internally the `MagicSettingsService` will be initialized automatically by the `MagicTheme` base class.
It will then go and pick up the JSON file, parse it, do a bunch of magic and come back with a final `MagicSettings` object.
This `MagicSettings` will then contain all the important settings for the current page/theme.
It will also keep a reference to other settings such as `Menus` for which many configurations can exist.

## How the Settings are Broadcast

A key feature of this system is that the settings are initially loaded in the theme,
and then broadcast to all controls used in that theme.

To make this happen, the theme must wrap everything in a `MagicContextAndSettings` tag:

```razor
<MagicContextAndSettings Settings="Settings">
  Content
</MagicContextAndSettings>
```

This will do a few things

1. Make sure that the inner content is only shown if Settings are loaded - otherwise show a `loading settings...` text
1. Broadcast the `MagicSettings` with the name `Settings` to all child controls using `CascadingValue`.
1. It will also ensure that the [MagicContext](./magic-context.md) is set on the page

TODO:
1. EXPLAIN SETTINGS MORE

## Continue...

Then continue back to the 👉🏾 [Home](../readme.md)
