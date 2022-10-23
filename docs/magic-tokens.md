---
title: Magic Tokens
permalink: /magic-tokens
icon: fa-key
order: 32
---

# cre8magic – Magic Tokens

Most settings will be parsed through a tokens-engine which will convert all kinds of tokens such as `[Page.Id]` into their respective value.

Note that each context is different.
For example, when parsing settings at the page level, `[Page...]` tokens will work, but `[Module...]` tokens will not work.

_Note: The list of tokens is still work in progress_

## Purpose of cre8magic Tokens

The purpose of these tokens is for use in configurable values - such as classes on HTML tags.

They are currently not going to give you all possible values, for eg. there is no `[Module.Title]` token
since there is no good reason to use this in such scenarios.

If you need those values in your HTML (eg. to create special hover-labels) you would just do that in Razor code.

## Site Tokens

Site tokens work everythere.
As of now we have these site tokens:

* `[Site.Id]` - ID of the current site

## Theme Tokens

Assets tokens work everywhere.
As of now we have these assets tokens:

* `[Theme.Url]` - the  url like `Themes/your-theme-name` for where your files should be
  note: it doesn't have leading or trailing slash, so you would use `[Theme.Path]/Assets/logo.svg`
  note: it also doesn't have `wwwroot` as that is never in the public URL

## Page Tokens

Page tokens work everywhere.
They are especially useful in creating menus.
As of now we have these page tokens:

* `[Page.Id]` - ID of the current page
* `[Page.ParentId]` - ID of the pages parent page or `none`
* `[Page.RootId]` - ID of the root page in the tree leading to this page or `none`

## Module Tokens

Module Tokens work on **Containers** only.
As of now we have these module tokens:

* `[Module.Id]` - ID of the current module

### Module Type Information

* `[Module.Namespace]` - the name like `Oqtane.Modules.HtmlText` of the current module
  ideal to add to containers where you wish to have special styling for special types of modules
  _this uses the namespace, without the final control name_
* `[Module.Control]` - the control name like `Index` of the current module
  ideal to specify even more specific styling
  _from the namespace the final control name_
* ~`[Module.Name]`~ - the name like `HtmlText` of the current module (from the namespace, without the final control name)
  _impossible to implement because each module will have different namespace conventions, so finding the real name isn't possible_

## Layout Tokens

TODO:

## Menu Tokens

Menu Tokens work on **Menus** only.
As of now we have these menu tokens:

* `[Menu.Id]` - ID of the menu which is normally randomly generated to ensure that each menu is unique for collapse/uncollapse
* `[Menu.Level]` - level of the menu which can be different for the page level, as menus that start at level 2 still have the first items on menu level 1

## How it Works

As of v0.1 2022-Q3 it's still a simple search-and-replace.
We plan to use a more powerfull RegEx in the near future.
