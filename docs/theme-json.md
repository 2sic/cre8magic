---
title: JSON Settings File
permalink: /json-settings-file
icon: fa-file
order: 21
---

# cre8magic – Magic Settings – JSON Settings File

cre8magic uses JSON to enable fast and flexible configuration of your theme.
It is used by [Magic Settings](./magic-settings.md) to load all the initial information.

This document explains the json file and everything you must know, such as:

1. Special tricks to make work easier such as comments
1. The general file format and sections
1. Named configurations
1. What can be configured in each section
1. Special nodes which can accept string or more complex data

## Example JSON

Here's a brief extract of such a configuration file
(here's a [live example](https://github.com/2sic/oqtane-theme-2shine-bs5/blob/main/Client/src/theme.json)):

```jsonc
{
  // By specifying a schema we get intellisense in this JSON
  "$schema": "https://2sic.github.io/cre8magic/schemas/2022-10/theme.json",

  "version": 0.01,

   // Global debug settings
  "debug": {
    "allowed": true,    // The most important setting - if this is false, nothing else will happen
    "admin": true,      // Enable everywhere for admin
    "anonymous": true,  // Enable everywhere for anonymous
  },

  // Theme Configurations
  "themes": {
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
1. Make sure you add the `"$schema"` node as you see above, to get help and instructions editing the JSON


## General File Format and Sections

### Sections

The `theme-settings.json` contains these primary nodes:

1. `version` _*_ (just version information)
1. `debug` _*_ for showing additional debug buttons on the page
1. `themes` for the main settings of a specific theme such as Logo, breadcrumbs on/off, etc.
1. `themeDesigns` has the configuration for CSS classes to be used in various places
1. `languages` contains configuration for languages to show in the menu
1. `menus` contains configuration for what menus show what nodes, like top-level with sub-level, etc.
1. `menuDesigns` contains a bunch of rules for how menus should be styled, such as classes to use on the active node etc.

### Named Configurations

All of the nodes above (except those marked with _*_) can have many different, named configurations.
So you'll see something like this:

```jsonc
{
  "themes": {
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


### @inherits Does Exactly What it Says

Example from the `menuDesigns`

```json
{
  "mobile": {
    "ul": {
      "byLevel": {
        "1": "navbar-nav",
        "-1": "collapse theme-submenu-[Menu.Id]-[Page.Id]"
      },
      "inBreadcrumb": "show"
    },
    "li": {
      "classes": "nav-item nav-[Page.Id] position-relative",
      "hasChildren": "has-child",
      "isActive": "active",
      "isDisabled": "disabled"
    },
    "a": {
      "classes": "nav-link mobile-navigation-link",
      "isActive": "active"
    },
    "span": {
      "classes": "nav-item-sub-opener",
      "inBreadcrumb": [ null, "collapsed" ]
    },
    // Special target information (not really styling) usually on the span-tag
    "data-bs-target": ".theme-submenu-[Menu.Id]-[Page.Id]"
  },
  "sidebar": {
    "@inherits": "Mobile",
    "a": {
      // This is the only difference to Mobile
      "classes": "nav-link"
    }
  }
}
```


### Short and Long Notations for True/False settings

Most properties which indicate a binary true/false value like `isActive` or `hasChildren` can be configured two ways:

* `"isActive": "some-class-when-active"`
* `"isActive": ["active-class", "not-active-class"]`
* `"isActive": [null, "not-active-class"]`


### Short and Long Notations for Complex Objects

Certain objects have a long notation, but can be shortened to just a string or bool if it's obvious what is meant. For example, the `parts` in the `themes` section would be:

```json
"breadcrumbs": {
  "show": true,
  "design": "special-design-name",
  "configuration": "special-config-name"
}
```

But this can be abbreviated to:

* `"breadcrumbs": true` - assumes show=true and design/configuration use the current name
* `"breadcrumbs": "name"` - assumes show=true and the design/config use the specified name

This setup also works for all the design settings where you can do:

* `"container": "some string"` - in this case, classes/value get this, everyhing else is empty


## Intellisense using $schema

To get help editing the file, add the `$schema` to your document.
Your editor (at least VS Code) will then help you fill in everything you need.

```json
"$schema": "https://2sic.github.io/cre8magic/schemas/2022-10/theme-configurations.schema.json"
```
