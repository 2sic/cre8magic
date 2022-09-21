using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Languages;

public abstract class MagicLanguageMenu: MagicControl
{
    [Inject] protected LanguageService LanguageService { get; set; }

    public List<MagicLanguage> Languages { get; private set; }

    /// <summary>
    /// Determines if the languages should be shown. Will be retrieved from the settings
    /// </summary>
    protected bool? Show { get; private set; } = null;

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