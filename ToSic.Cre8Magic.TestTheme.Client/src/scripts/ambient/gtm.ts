import { debug } from '../shared/constants';

// Temporary solution to add GTM, not final!
const debugGtm = debug || true;

function activateGtm(gtmKey: string) {
  if (debugGtm) console.log('2dm test activate GTM');
// <!-- Google Tag Manager -->
// <script>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer',gtmKey);
// </script>
// <!-- End Google Tag Manager -->
}

class GtmInterop {
  activate = activateGtm;
  pageView() {
    if (debugGtm) console.log('gtm-interop - track page view');
    gtag('event', 'blazor_page_view');//
  }
}

window.dataLayer = window.dataLayer || [];
function gtag(target: 'event', more: unknown) {
  if (debug) console.log('gtm - gtag');
  window.dataLayer.push(arguments);
}

declare global {
  interface Window {
    cre8magic: { gtm: GtmInterop },
    dataLayer: unknown[],
    gtag(): void,
  }
}

export function buildCre8magicGtm() {
  window.cre8magic = window.cre8magic || {};
  window.cre8magic.gtm = window.cre8magic.gtm || new GtmInterop();
  window.gtag = window.gtag || gtag;
}