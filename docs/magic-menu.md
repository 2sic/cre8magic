---
title: Magic Menu
permalink: /magic-menu
icon: fa-bars
order: 33
---

# cre8magic – Magic Menu

A core challenge with any website is creating great menus.
There are actually three distinct problems to solve:

1. Managing multiple menus on the same page
    * the main menu
    * possibly a side-menu with sub-pages
    * a footer menu for disclaimer and privacy
    * multiple menus in the footer for mega-footers with many links

1. **Configuration** for selecting the pages which should appear in the menu
    * where to start (like a menu which start at level 2)
    * what pages to show (like all the pages on level 2 - or only their children)
    * how deep to go (do we show submenus?)

1. **Design** for styling of each node based on the context
    * is the current node selected? add `active`...
    * is the current node a parent of the selected node? add `is-parent`...
    * is the current node a dropdown for pages beneath it...

## Manage Multiple Menus

The MagicMenu gives each menu a name, such as `Main`, `Sidebar`, `Footer` etc.
You can determine these names in the Razor files.

Each of these menus can then be configured in the [JSON](./theme-json.md).
By default, each menu will find it's **configuration** and it's **design**
based on the same name.
So the `Main` menu would take the configuration and design called `Main`.

But you can also reconfigure this.
For example, you could say that the Theme `Sidebar` will use
the configuration `TopLevelOnly` for the `Main` menu.
This is configured in the `parts` of the `themes` section of the JSON file.

## Menu Configuration

The menu configuration determines some important aspects such as

* What node to `start` from - eg. `*` is the top level, `.` is the current page
* What to do from the start - like `"children": true` means
"begin with the children" of the start-node
* What level to show - so `"start": ".", "level": -1` means to start
one level above the current page
* How deep to go, so `"depth": 2` would show the starting level and one more

All this is configured in the `menus` section of the JSON.

### Start Values

These are accepted values of the node `start`:

* `*` root
* `.` current page
* `42` the page 42
* `5!` the page 5 even if it's normally not visible in a menu
* `42, 5!` combinations thereof

The following parameters will also influece what is shown on the first level:

* `"start": ".", "children": true` starts with children of the current page
* `"start": "42", "children": true` starts with children of page 42 - ideal for footer or system-menus
* `"start": ".", "level": 2` starts with the page on level 2 which is above the current page
* `"start": ".", "level": -1` starts with the page one level above the current page
* you can also combine start=. level=-1 and children=true for further desired effects

### Depth

The depth must always be at least 1 and determines how many levels downwards the nodes are rendered.

## Menu Design

This is one of the most sophisticated bits of the JSON settings.
You can configure this in the `menuDesigns` section of the JSON.
Note that this uses the [Magic Classes](./magic-values.md) and [Magic Tokens](./magic-tokens.md).
Example:

```jsonc
"menuDesigns": {
  "Mobile": {
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
}
```

This means a lot of things, but let's highlight some aspects:

1. the surrounding `<ul>` tag will get the `navbar-nav` class at the first level; all others will get `collapse` and others
1. the `<ul>` will also get a menu and page specific class because of the `theme-submenu-[Menu.Id]-[Page.Id]` which is useful for the collapse identification in bootstrap
1. the `<li>` of each node will get some classes including an `active` if it's the current page, and `has-child` if it has children so that the bootstrap menu will do it's magic
1. the `<a>` link itself will also have different classes based on active
1. the `<span>` is used to show a `+`/`-` indicator using the `nav-item-sub-opener`
1. ...and it will also get's `collapsed` if it's not in the breadcrumb (so it's only opened if a sub-page is the current page)
1. and a special attribute used by bootstrap `data-bs-target` will have the same contents as the identifying class of the surrounding `<ul>` to ensure bootstrap will work

---

## Missing Features

1. As of now you cannot filter out specific pages.
  For this you would still need to write your own code or construct your nav-tree for special cases.
1. You cannot link to page in another language, as Oqtane doesn't really have this concept yet.

## History

1. Added in v0.0.1 2022-10 with 80% coverage of what DDR Menu had in DNN
