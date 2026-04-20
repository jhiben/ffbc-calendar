import { writable } from 'svelte/store';
import type { Event, GeoCoord } from './types';
import { fetchEvents, geocode } from './api';

function createEventsStore() {
	const { subscribe, set } = writable<Event[]>([]);
	let loaded = false;
	let loading: Promise<Event[]> | null = null;

	async function load(): Promise<Event[]> {
		if (loaded) {
			let current: Event[] = [];
			subscribe((v) => (current = v))();
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

	return { subscribe, load };
}

export const events = createEventsStore();

const geocodeCache = new Map<string, GeoCoord | null>();

export async function cachedGeocode(postalCode: string, country?: string): Promise<GeoCoord | null> {
	const key = `${postalCode}:${country ?? ''}`;
	if (geocodeCache.has(key)) return geocodeCache.get(key)!;
	const result = await geocode(postalCode, country ?? undefined);
	geocodeCache.set(key, result);
	return result;
}
