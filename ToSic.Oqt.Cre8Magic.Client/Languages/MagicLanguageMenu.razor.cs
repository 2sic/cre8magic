using Microsoft.AspNetCore.Components;
using ToSic.Oqt.Cre8Magic.Client.Controls;

namespace ToSic.Oqt.Cre8Magic.Client.Languages;

public abstract class MagicLanguageMenu: MagicControl
{
    [Inject] private LanguageService LanguageService { get; set; }

    public List<MagicLanguage> Languages { get; private set; }

    public bool? Show { get; private set; } = null;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        LanguageService.InitSettings(Settings);

        // Load defined language list. It change unless the page is reloaded, so we can cache it on this control
        Languages ??= await LanguageService.LanguagesToShow(PageState.Site.SiteId);
        Show ??= await LanguageService.ShowMenu(PageState.Site.SiteId);
    }

    public async Task SetLanguage(string culture) => await LanguageService.SetCultureAsync(culture);



    public string? Classes(string tag, MagicLanguage? lang = null) => Settings.LanguageDesign.Classes(tag, lang).EmptyAsNull();
}