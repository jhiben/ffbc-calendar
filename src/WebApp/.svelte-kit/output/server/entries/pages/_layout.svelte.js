import { L as attr, R as escape_html, a as head, i as ensure_array_like, n as attr_class } from "../../chunks/dev.js";
import { t as page } from "../../chunks/state.js";
//#region src/routes/+layout.svelte
function _layout($$renderer, $$props) {
	$$renderer.component(($$renderer) => {
		let { children } = $$props;
		const navLinks = [
			{
				href: "/list",
				label: "List"
			},
			{
				href: "/calendar",
				label: "Calendar"
			},
			{
				href: "/map",
				label: "Map"
			}
		];
		let navOpen = false;
		head("12qhfyh", $$renderer, ($$renderer) => {
			$$renderer.title(($$renderer) => {
				$$renderer.push(`<title>FFBC Calendar</title>`);
			});
		});
		$$renderer.push(`<header><nav class="navbar navbar-expand-sm navbar-dark"><div class="container"><a class="navbar-brand" href="/">FFBC Calendar</a> <button class="navbar-toggler" type="button" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button> <div${attr_class("collapse navbar-collapse", void 0, { "show": navOpen })}><ul class="navbar-nav ms-auto"><!--[-->`);
		const each_array = ensure_array_like(navLinks);
		for (let $$index = 0, $$length = each_array.length; $$index < $$length; $$index++) {
			let link = each_array[$$index];
			$$renderer.push(`<li class="nav-item"><a${attr_class("nav-link", void 0, { "active": page.url.pathname === link.href })}${attr("href", link.href)}>${escape_html(link.label)}</a></li>`);
		}
		$$renderer.push(`<!--]--></ul></div></div></nav></header> <main class="container py-4">`);
		children($$renderer);
		$$renderer.push(`<!----></main> <footer class="footer text-center"><div class="container">© ${escape_html((/* @__PURE__ */ new Date()).getFullYear())} FFBC Calendar · <a href="/feed.xml" class="text-muted">RSS Feed</a></div></footer>`);
	});
}
//#endregion
export { _layout as default };
