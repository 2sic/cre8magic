using Microsoft.JSInterop;
using ToSic.Cre8Magic.Client.JsModules;

namespace ToSic.Cre8Magic.Client.Services;

/// <summary>
/// Constants and helpers related to JS calls from the Theme to it's own JS libraries
/// </summary>
// TODO: SOME DAY move to Cre8Magic, as soon as we know how to reliably include the js-assets in the final distribution
public class MagicThemeJsServiceTest : MagicJsServiceBase, IMagicThemeJsService
{

    public MagicThemeJsServiceTest(IJSRuntime jsRuntime) : base(jsRuntime, $"./_content/{MagicConstants.PackageId}/test.js")
    {
    }

    /// <inheritdoc />
    public async Task SetBodyClasses(string classes) 
    {
        await InvokeAsync<string>("setBodyClass", classes);
    }

    public async Task<string> TestFromTest()
    {
        return await InvokeAsync<string>("testFromTest");
    }
}