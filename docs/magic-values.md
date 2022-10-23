---
title: Magic Values
permalink: /magic-values
icon: fa-heart
order: 31
---

# cre8magic – Magic Values, Classes and More

Almost all design work is done using very few changes to the HTML.
The only thing we usually must do, is:

1. set some `id` properties
1. set some `class` properties - often based on the context (so the pane may need `pane-is-empty`)
1. set some values - such as `data-bs-toggle`

**cre8magic** makes this happen using these parts:

1. The [theme.json](./theme-json.md) which has all the configurations
1. The [Magic Settings](./magic-settings.md) which will parse the json and provide the parts we need
1. Various _Desiger_ helpers which will do some magic and add context
1. The [Magic Tokens](./magic-tokens.md) which will replace things such as `[Page.Id]` if it was used in class strings
1. Simple helper methods such as `Classes(name)`, `Value(name)` or `Id(name)` on all the Magic Razor base classes like the MagicMenu, MagicBreadcrumbs or MagicContainer

## How to Use

Basically all your controls must usually do is write HTML along these lines:

```html
@inherits MagicContainer
<!-- some code parts skipped for brevity -->
<div id='@Id("container")' class='@Classes("container")'>
    <div class="container">
        <Oqtane.Themes.Controls.ModuleActions/>
        <ModuleInstance/>
    </div>
</div>
```

...and of course make sure the values for the above mentioned `container` exist in the [theme.json](./theme-json.md).

Everything else just works magically.

## Razor API

All the `Magic*` Razor controls have the following methods to make like easier:

1. Classes(name)
1. Value(name)
1. Id(name)

A few have some extra methods, such as these:

1. The `MagicTheme` also has a `PaneClasses(name)` to also add something to change styling if the pane is empty

---

## History

1. All created in v0.0.1 2022-10
