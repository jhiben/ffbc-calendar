import { R as escape_html, a as head, r as derived } from "../../../chunks/dev.js";
import { t as page } from "../../../chunks/state.js";
import "../../../chunks/stores.js";
//#region src/routes/event/+page.svelte
function _page($$renderer, $$props) {
	$$renderer.component(($$renderer) => {
		let event = null;
		derived(() => page.url.searchParams.get("date"));
		derived(() => page.url.searchParams.get("title"));
		head("lfjtkv", $$renderer, ($$renderer) => {
			$$renderer.title(($$renderer) => {
				$$renderer.push(`<title>FFBC Calendar — ${escape_html(event?.title ?? "Event Detail")}</title>`);
			});
		});
		$$renderer.push("<!--[0-->");
		$$renderer.push(`<div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>`);
		$$renderer.push(`<!--]-->`);
	});
}
//#endregion
export { _page as default };
