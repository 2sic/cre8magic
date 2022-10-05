# Cre8Magic – Magic Settings

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

## Example File

Here's a brief extract of such a configuration file:

```jsonc
{
  // Theme Configurations
  "Themes": {
    // Default Theme - in most cases you'll just use this theme configuration
    "Default": {
      // Optional: Design Names with "=" mean they inherit / use the name already existing - in this case "Default"
      "design": "=",
      "logo": "[Theme.Url]/assets/logo.svg",
      "languagesMin": 1,
      "parts": {
        // Show breadcrumbs. Note that on home it won't be visible due to CSS rules + MagicContext)
        "breadcrumbs": true,
        // Show languages menu and use the same languages configuration name as this theme - in this case "Default"
        "languages": true,
        // Don't show sidebar menu in the default configuration
        "menu-sidebar": false,
        // Example of a more detailed setting in case you want to control everything
        "example-part-config": {
          "show": true,
          "design": "=",
          "configuration": "special-config",
        }
      },
      // Determins if we should use the body (or a div) for the magic context
      // As of now we feel that browser behavior is better if it's in the div-tag (false)
      "magicContextInBody": false,
      "debug": {
        "admin": true,
      }
    },
  }
}
```

_Note: the file also supports comments and trailing commas, so you can really work!_

## The Configuration File

The system works by creating a json file such as `theme-configuration.json`.

This is placed in your themes `wwwroot` folder like in `/wwwroot/ToSic.Themes.BlazorCms/theme-configuration.json`

Which file to use can be configured in the theme. 
Normally you would use the same file for all variations of your theme, but the important thing is that the theme
must give some initial configuration object to the Cre8Magic Services. 

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

## The Settings Json

The settings json contains various primary nodes such as:

1. `Version` _*_ (just version information)
1. `Debug` _*_ for showing additional debug buttons on the page
1. `Themes` for the main settings of a specific theme such as Logo, breadcrumbs on/off, etc.
1. `ThemeDesigns` has the configuration for CSS classes to be used in various places
1. `Languages` contains configuration for languages to show in the menu
1. `Menus` contains configuration for what menus show what nodes, like top-level with sub-level, etc.
1. `MenuDesigns` contains a bunch of rules for how menus should be styled, such as classes to use on the active node etc.

_Note that the node names are case insensitive, so `Debug` or `debug` will both work._

All of the nodes above (except those marked with _*_) can have many different, named configurations. 
So you'll see something like this:

```jsonc
{
  "Menus": {
    // The default configuration is taken if no other configuration is used
    "Default": {
      "start": "*",
      "depth": 1,
    },
    // This is used as the main-menu in the navigation with sidebar submenu
    "ToplevelOnly": {
      "start": "*",
      "depth": 1
    },
  }
}
```

This means that there is a configuration called `Default` and one called `TopLevelOnly` which will do somehing different. 

Each section will have different settings which we haven't documented in detail, 
but do check out the ToShine Template Theme as it shows everything in action. 

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
1. It will also ensure that the `MagicContext` TODO: is set on the page

TODO:
1. EXPLAIN SETTINGS MORE
1. LIKE FOR MENUS ETC.?
1. EXPLAIN SETTINGS PARTS

## Continue...

Then continue back to the 👉🏾 [Home](../readme.md)