import type { Event, EventDetails, GeoCoord } from './types';

const BASE = '';

export async function fetchEvents(): Promise<Event[]> {
	const res = await fetch(`${BASE}/api/events`);
	if (!res.ok) throw new Error(`Failed to fetch events: ${res.status}`);
	return res.json();
}

export async function fetchEventDetails(eventId: string): Promise<EventDetails | null> {
	const res = await fetch(`${BASE}/api/events/${encodeURIComponent(eventId)}/details`);
	if (res.status === 404) return null;
	if (!res.ok) throw new Error(`Failed to fetch event details: ${res.status}`);
	return res.json();
}

export async function geocode(postalCode: string, country?: string): Promise<GeoCoord | null> {
	const params = country ? `?country=${encodeURIComponent(country)}` : '';
	const res = await fetch(`${BASE}/api/geocode/${encodeURIComponent(postalCode)}${params}`);
	if (res.status === 404) return null;
	if (!res.ok) throw new Error(`Failed to geocode: ${res.status}`);
	return res.json();
}
