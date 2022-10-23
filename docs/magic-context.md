---
title: Magic Context
permalink: /magic-context
icon: fa-magic
order: 30
---

# cre8magic – Magic Context

The Magic Context adds a bunch of classes to the `<body>` or a wrapper `<div>` tag
which contain information about the current state.

This allows you to then write CSS rules, which change the look / feel of the page based on certain aspects.

## Example

The wrapper `<div>` could look like this:

```html
<div id="cre8magic-root" class="page-35 page-root-29 page-parent-33 site-1 nav-level-3 theme-mainnav-variation-right theme-variation-centered">
  <!-- the contents of the page-->
</div>
```

This tells you a bunch of things such as:

1. This is page `#35` - allows you to do things like special colors for exactly this page
1. The page is in the tree of the root `#29` - so you could use special colors for all the pages in this branch
1. The pages parent is `#33` - again letting you write custom CSS
1. We are on site `#1` - in case you have different styles based on the site number
1. We are currently on the nav-level `3` - this could affect your breadcrumb or something
1. We are in a specific layout type and navigation type

## How this Works

Basically all this happens automatically, if your [json configuration](./theme-json.md) has `magicContext` of something like this:

```jsonc
{
  "magicContext": [
    "page-[Page.Id]",
    "page-root-[Page.RootId]",
    "page-parent-[Page.ParentId]",
    "site-[Site.Id]",
    "nav-level-[Menu.Level]",
    "theme-mainnav-variation-right",
    "theme-variation-[Layout.Variation]"
  ]
}
```

You can of course add your own rules with other placeholders to make this happen.

## Configuration

1. You can determine if this will be added to a `<div>` or the `<body>` using the setting `magicContextInBody`
    * If it's on the body, it will be put there using JavaScript, so there may be a minimal flash-of-unexpected-styling
    * if you put it in the div, it will always be there
1. You can also change the `id` of the `<div>` tag if you want to change how your CSS behaves. The `id` can be set on the `magicContextTagId`

## Missing Features

As of 2022-10 there are no magic context classes for languages yet, as Oqtane doesn't fully support Multi-Language.
We'll add it as soon as possible.

---

## History

1. Added in v0.0.1 2022-10
