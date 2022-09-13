# Cre8Magic – How It Works

**Magic Settings** allow you to move 95% of the theme code into some kind of configuration. 

Below we'll give you an example of what you would normally have, and how it would be done with Cre8Magic.

## Challenge: Smart Containers

Let's assume you have a container which is a bunch of `div` tags and a bit of CSS. 
In this example we have two features we are using

1. a special ID for CSS targeting (for special cases where we wish to have CSS for a very specific module)
1. some CSS classes which could vary depending on certain factors - such as if it's unpublished to show something is wrong

## Classic Solution

```razor
@inherits Oqtane.Themes.ContainerBase
<div id='module-@ModuleState.ModuleId' class='to-shine-background-container py-4 @(CheckIfModulePublished() ? "" : "module-unpublished") @(ModuleState.UseAdminContainer ? "to-shine-admin-container" : "...")'>
    <div class="container">
        <Oqtane.Themes.Controls.ModuleActions/>
        <ModuleInstance/>
    </div>
</div>
@code
{
  public bool CheckIfModulePublished()
  {
    return UserSecurity.ContainsRole(ModuleState.Permissions, PermissionNames.View, RoleNames.Everyone);
  }
}
```

Based on this example you can see, that there is a mix of logic and design which is 

1. hard to read
1. hard for a designer to develop
1. error prone
1. hard to maintain

## Simple with Cre8Magic

Here's how it works with Cre8Magic:

```razor
@inherits MagicContainer
<div id='@Value("Id")' class='@Classes("div")'>
    <div class="container">
        <Oqtane.Themes.Controls.ModuleActions/>
        <ModuleInstance/>
    </div>
</div>
```

For the system to know what it should do, there are **Magic Settings** which are easy to manage. 
Below we're only showing the settings relevant to this example, there are of course more:

```json
// ********************
// Containers has properties / settings / values on containers which are not directly related to design
"Containers": {
  "Default": {
    "Values": {
      "id": "module-[Module.Id]"
    }
  }
},
// ********************
// Container Designs determine CSS classes on containers
"ContainerDesigns": {
  "Default": {
    "div": {
      "classes": "to-shine-background-container py-4",
      // "isPublished": "module-published",
      "isNotPublished": "module-unpublished",
      "isAdminModule": "to-shine-admin-container"
      // "isNotAdminModule": "to-shine-default-container"
    }
  }
},
```
## The Magic in the Background

Cre8Magic will do a bunch of things in the background, such as:

1. Load configuration from
    1. the DB targeting the page (WIP)
    1. the DB targeting a branch in the menu (WIP)
    1. the DB targeting the site (WIP)
    1. JSON targeting all kinds of scenarios
    1. defaults in your code
    1. final defaults in Cre8Magic
1. Flatten configurations to match the current theme
    1. Use names to find the configuration for the theme
    1. Use further names to find the configuration for each part, such as Container, ContainerDesign, etc.
    1. Flatten all to the current scenario
1. Broadcast the flattened settings at Theme-level
    1. Initialize the proper settings
    1. Broadcast these settings to all controls that are somewhere within the theme object tree
1. Provide simple accessors
    1. The `MagicTheme`, `MagicContainer`, `MagicControl` etc. all pick up the settings automatically
    1. ...and have special APIs such as `@Classes(...)` helpers to retrieve the values
1. Process Tokens
    1. Settings can contain tokens such as `[Module.Id]` which will be rendered into the final result

## TL;DR

You got this far? Let's go back to home and start designing! 

👉🏾 [Home](../readme.md)