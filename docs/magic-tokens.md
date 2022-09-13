# Cre8Magic – Magic Tokens

Most settings will be parsed through a tokens-engine which will convert all kinds of tokens such as `[Page.Id]` into their respective value. 

Note that each context is different. 
For example, when parsing settings at the page level, `[Page...]` tokens will work, but `[Module...]` tokens will not work.

_Note: The list of tokens is still work in progress_

## Site Tokens

Site tokens work everythere. 
As of now we have these site tokens:

* `[Site.Id]` - ID of the current site

## Theme / Assets / Resources Tokens

Assets tokens work everywhere.
As of now we have these assets tokens:

* `[Theme.Path]` - WIP!
* `[Theme.AssetsPath]` - WIP! the path to the `wwwroot/theme-name/assets` for where your files should be  
  note that it doesn't have a trailing slash, so you would use `[Theme.AssetsPath]/logo.svg`

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
* `[Module.Control]` - the control name like `Index` of the current module (from the namespace the final control name)
* ~`[Module.Name]` - the name like `HtmlText` of the current module (from the namespace, without the final control name)~  
  _we don't implement this on purpose, because each module will have different namespace implementations so finding the name isn't possible
* `[Module.Namespace]` - the name like `Oqtane.Modules.HtmlText` of the current module  
  ideal to add to containers where you wish to have special styling for special types of modules
  _this uses the namespace, without the final control name_

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