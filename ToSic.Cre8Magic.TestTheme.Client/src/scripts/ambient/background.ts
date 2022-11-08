import { debug, prefix } from './../shared/constants';
import { initMailDecrypt } from './mail-encrypt';
import { initOffCanvasEvents } from './menu';
import { initBreadcrumb } from './breadcrumbs';
import { openPdfInNewWindow } from './pdf-in-new-window';
import { initToTop } from './to-top';
import { buildCre8magicGtm } from './gtm';

initToTop();

openPdfInNewWindow();

initMailDecrypt();

initBreadcrumb();

initOffCanvasEvents();

buildCre8magicGtm();
window.cre8magic.gtm.activate('GTM-T8W5TBL');