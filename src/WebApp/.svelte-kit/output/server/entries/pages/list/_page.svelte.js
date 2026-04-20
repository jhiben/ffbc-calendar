import { a as head } from "../../../chunks/dev.js";
import "../../../chunks/stores.js";
//#region src/routes/list/+page.svelte
function _page($$renderer, $$props) {
	$$renderer.component(($$renderer) => {
		head("3n1c4l", $$renderer, ($$renderer) => {
			$$renderer.title(($$renderer) => {
				$$renderer.push(`<title>FFBC Calendar — Events List</title>`);
			});
		});
		$$renderer.push(`<h2 class="section-heading">Events</h2> `);
		$$renderer.push("<!--[0-->");
		$$renderer.push(`<div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>`);
		$$renderer.push(`<!--]-->`);
	});
}
//#endregion
export { _page as default };
