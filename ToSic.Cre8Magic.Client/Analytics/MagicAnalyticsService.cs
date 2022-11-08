using Microsoft.JSInterop;
using static ToSic.Cre8Magic.Client.Services.DoStuff;

namespace ToSic.Cre8Magic.Client.Analytics
{
    public class MagicAnalyticsService
    {
        private const string GtmEvent = "event";
        public MagicAnalyticsService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }
        public IJSRuntime JsRuntime { get; }

        /// <summary>
        /// Must run in OnAfterRenderAsync for now
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        internal async Task TrackPage(MagicSettings? settings, bool firstRender)
        {
            if (settings == null) return;
            var gtmSettings = settings.Analytics;
            if (gtmSettings.PageViewTrack != true) return;

            if (firstRender && gtmSettings.PageViewTrackFirst != true) return;
            var js = gtmSettings.PageViewJs!;
            var eventName = gtmSettings.PageViewEvent;

            // Run the JS Command but don't wait for it
            // https://stackoverflow.com/questions/17805887/using-async-without-await
            await DoNotWait(() => IgnoreError(() => JsRuntime.InvokeVoidAsync(js, GtmEvent, eventName)));
        }
    }
}
