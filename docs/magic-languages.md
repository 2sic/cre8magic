---
title: Magic Languages
permalink: /magic-languages
icon: fa-language
order: 33
---

# cre8magic – Magic Languages

The languages need three things to work as expected:

1. They need to know if they should show
1. They need to know what languages to show and what labels to use
1. They need to be styled

**cre8magic** makes this happen using these parts:

1. The [theme.json](./theme-json.md) which has all the configurations
    1. The `themes` section determines if the menu should show using `parts.languages`
    1. The `themes` section also has a `languagesMin` which would hide the languages if there are less than X languages (usually it needs at least 2)
    1. The `languages` section has configurations for what languages to show and what labels to use
    1. The `themeDesigns` section determines how it should look - ATM there is only one key `languages-li` since the rest has sufficient automatic class names to cover all known styling cases
1. The [Magic Settings](./magic-settings.md) which will parse the json and provide the parts we need
1. The base Razor `MagicLanguages` prepares everything so you can inherit from it and create the output as needed

## How to Use

Best check the reference implementation on 2shine Template Theme

---

## History

1. All created in v0.0.1 2022-10
