# Cre8Magic – Magic Settings – JSON Settings File

Cre8Magic uses JSON to enable fast and flexible configuration of your theme. 
It is used by [Magic Settings](./magic-settings.md) to load all the initial information.

This document explains the json file and everything you must know, such as:

1. Special tricks to make work easier such as comments
1. The general file format and sections
1. Named configurations
1. What can be configured in each section
1. Special nodes which can accept string or more complex data

## Example JSON

Here's a brief extract of such a configuration file 
(here's a [live example](https://github.com/2sic/oqtane-theme-2shine-bs5/blob/main/Client/src/theme-settings.json)):

```jsonc
{
  "version": 0.01,

   // Global debug settings
  "debug": {
    "allowed": true,    // The most important setting - if this is false, nothing else will happen
    "admin": true,      // Enable everywhere for admin
    "anonymous": true,  // Enable everywhere for anonymous
  }, 

  // Theme Configurations
  "Themes": {
    // Default Theme - in most cases you'll just use this theme configuration
    "Default": {
      // Optional: Design Names with "=" mean they inherit / use the name already existing - in this case "Default"
      "design": "=",
      "logo": "[Theme.Url]/assets/logo.svg",
      "languagesMin": 1,
      "parts": {
        "breadcrumbs": true,
        // ...more stuff here...
      },
      // Determins if we should use the body (or a div) for the magic context
      // As of now we feel that browser behavior is better if it's in the div-tag (false)
      "magicContextInBody": false,
      "debug": {
        "admin": true,
      }
    },
  },

  // ...more stuff here...
}
```


## Special Tricks in the File

The file will be parsed by .net, but we've activated some special features to make life easier:

1. Comments are allowed
1. Trailing commas are allowed
1. Almost all node names are case insensitive, so `"Default"` and `"default"` are treated the same


## General File Format and Sections

### Sections

The `theme-settings.json` contains these primary nodes:

1. `Version` _*_ (just version information)
1. `Debug` _*_ for showing additional debug buttons on the page
1. `Themes` for the main settings of a specific theme such as Logo, breadcrumbs on/off, etc.
1. `ThemeDesigns` has the configuration for CSS classes to be used in various places
1. `Languages` contains configuration for languages to show in the menu
1. `Menus` contains configuration for what menus show what nodes, like top-level with sub-level, etc.
1. `MenuDesigns` contains a bunch of rules for how menus should be styled, such as classes to use on the active node etc.

### Named Configurations

All of the nodes above (except those marked with _*_) can have many different, named configurations. 
So you'll see something like this:

```jsonc
{
  "Themes": {
    // The default configuration is taken if no other configuration is used
    "Default": {
      // ...stuff...
    },
    // Alternate configuration for theme with menu on the side
    "Sidebar": {
      // ...stuff...
    },
  }
}
```

This means that there is a configuration called `Default` and one called `Sidebar` which will do something different. 



TODO:
1. LIKE FOR MENUS ETC.?
1. EXPLAIN SETTINGS PARTS
