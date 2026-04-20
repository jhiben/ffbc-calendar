import { L as attr, R as escape_html, a as head, r as derived, s as stringify } from "../../../chunks/dev.js";
import { t as page } from "../../../chunks/state.js";
import "../../../chunks/stores.js";
//#region src/routes/calendar/+page.svelte
function _page($$renderer, $$props) {
	$$renderer.component(($$renderer) => {
		let allEvents = [];
		const searchParams = derived(() => page.url.searchParams);
		const displayYear = derived(() => Number(searchParams().get("year")) || (/* @__PURE__ */ new Date()).getFullYear());
		const displayMonth = derived(() => Number(searchParams().get("month")) || (/* @__PURE__ */ new Date()).getMonth() + 1);
		const monthName = derived(() => new Date(displayYear(), displayMonth() - 1).toLocaleString("en-GB", { month: "long" }));
		const prevMonth = derived(() => displayMonth() === 1 ? 12 : displayMonth() - 1);
		const prevYear = derived(() => displayMonth() === 1 ? displayYear() - 1 : displayYear());
		const nextMonth = derived(() => displayMonth() === 12 ? 1 : displayMonth() + 1);
		const nextYear = derived(() => displayMonth() === 12 ? displayYear() + 1 : displayYear());
		const today = /* @__PURE__ */ new Date();
		const todayYear = today.getFullYear();
		const todayMonth = today.getMonth() + 1;
		const todayDay = today.getDate();
		derived(() => {
			const firstDay = new Date(displayYear(), displayMonth() - 1, 1);
			const daysInMonth = new Date(displayYear(), displayMonth(), 0).getDate();
			let startDow = (firstDay.getDay() + 6) % 7;
			const monthEvents = allEvents.filter((e) => {
				const d = new Date(e.date);
				return d.getFullYear() === displayYear() && d.getMonth() + 1 === displayMonth();
			});
			const rows = [];
			let currentDay = 1;
			while (currentDay <= daysInMonth) {
				const week = [];
				for (let col = 0; col < 7; col++) if (rows.length === 0 && col < startDow || currentDay > daysInMonth) week.push({
					date: null,
					day: null,
					events: [],
					isToday: false
				});
				else {
					const day = currentDay;
					const cellDate = new Date(displayYear(), displayMonth() - 1, day);
					const dayEvents = monthEvents.filter((e) => new Date(e.date).getDate() === day);
					const isToday = displayYear() === todayYear && displayMonth() === todayMonth && day === todayDay;
					week.push({
						date: cellDate,
						day,
						events: dayEvents,
						isToday
					});
					currentDay++;
				}
				rows.push(week);
			}
			return rows;
		});
		head("13luymz", $$renderer, ($$renderer) => {
			$$renderer.title(($$renderer) => {
				$$renderer.push(`<title>FFBC Calendar — ${escape_html(monthName())} ${escape_html(displayYear())}</title>`);
			});
		});
		$$renderer.push(`<div class="d-flex justify-content-between align-items-center mb-3"><a${attr("href", `/calendar?year=${stringify(prevYear())}&month=${stringify(prevMonth())}`)} class="btn btn-outline-secondary">« Prev</a> <h2 class="mb-0">${escape_html(monthName())} ${escape_html(displayYear())}</h2> <a${attr("href", `/calendar?year=${stringify(nextYear())}&month=${stringify(nextMonth())}`)} class="btn btn-outline-secondary">Next »</a></div> `);
		$$renderer.push("<!--[0-->");
		$$renderer.push(`<div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>`);
		$$renderer.push(`<!--]-->`);
	});
}
//#endregion
export { _page as default };
