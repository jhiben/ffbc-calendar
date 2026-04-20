import { T as writable } from "./dev.js";
import "./index-server2.js";
//#region src/lib/api.ts
var BASE = "";
async function fetchEvents() {
	const res = await fetch(`${BASE}/api/events`);
	if (!res.ok) throw new Error(`Failed to fetch events: ${res.status}`);
	return res.json();
}
async function geocode(postalCode, country) {
	const params = country ? `?country=${encodeURIComponent(country)}` : "";
	const res = await fetch(`${BASE}/api/geocode/${encodeURIComponent(postalCode)}${params}`);
	if (res.status === 404) return null;
	if (!res.ok) throw new Error(`Failed to geocode: ${res.status}`);
	return res.json();
}
//#endregion
//#region src/lib/stores.ts
function createEventsStore() {
	const { subscribe, set } = writable([]);
	let loaded = false;
	let loading = null;
	async function load() {
		if (loaded) {
			let current = [];
			subscribe((v) => current = v)();
			return current;
		}
		if (loading) return loading;
		loading = fetchEvents().then((events) => {
			set(events);
			loaded = true;
			loading = null;
			return events;
		});
		return loading;
	}
	return {
		subscribe,
		load
	};
}
var events = createEventsStore();
var geocodeCache = /* @__PURE__ */ new Map();
async function cachedGeocode(postalCode, country) {
	const key = `${postalCode}:${country ?? ""}`;
	if (geocodeCache.has(key)) return geocodeCache.get(key);
	const result = await geocode(postalCode, country ?? void 0);
	geocodeCache.set(key, result);
	return result;
}
//#endregion
export { events as n, cachedGeocode as t };
