import "../../../chunks/index-server.js";
import { a as head, n as attr_class } from "../../../chunks/dev.js";
import "../../../chunks/stores.js";
//#region src/routes/map/+page.svelte
function _page($$renderer, $$props) {
	$$renderer.component(($$renderer) => {
		let loading = true;
		head("w85nl5", $$renderer, ($$renderer) => {
			$$renderer.title(($$renderer) => {
				$$renderer.push(`<title>FFBC Calendar — Map</title>`);
			});
		});
		$$renderer.push(`<h2 class="section-heading">Event Map</h2> `);
		$$renderer.push("<!--[0-->");
		$$renderer.push(`<div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>`);
		$$renderer.push(`<!--]--> <div id="map"${attr_class("", void 0, { "d-none": loading })}></div> `);
		$$renderer.push("<!--[-1-->");
		$$renderer.push(`<!--]-->`);
	});
}
//#endregion
export { _page as default };
