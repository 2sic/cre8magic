# Cre8Magic – Magic Settings

**Magic Settings** allow you to move 95% of the theme code into some kind of configuration. 

## Example

Let's assume you have a container which is a bunch of `div` tags and a bit of CSS. 
In this example we have two features we are using

1. a special ID for CSS targeting (for special cases where we wish to have CSS for a very specific module)
1. some CSS classes which could vary depending on certain factors - such as if it's unpublished to show something is wrong

```razor
@inherits Oqtane.Themes.ContainerBase
<div id='module-@ModuleState.ModuleId' class='to-shine-background-container py-4 @(CheckIfModulePublished() ? "" : "module-unpublished")'>
    <div class="container">
        <Oqtane.Themes.Controls.ModuleActions/>
        <ModuleInstance/>
    </div>
</div>
@code
{
  public bool CheckIfModulePublished()
  {
    // details skipped here to keep the sample clean - but it's complex ;)
  }
}
```

Based on this example you can see, that there is a mix of logic and design which is 

1. hard to read
1. hard for a designer to develop and maintain
1. error prone

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
// Container determine properties / values on containers which are not directly related to design
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
    }
  }
},
```

TODO: