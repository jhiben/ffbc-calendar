<script lang="ts">
	import { onMount } from 'svelte';
	import { events, cachedGeocode } from '$lib/stores';
	import type { Event } from '$lib/types';

	let allEvents = $state<Event[]>([]);
	let unmappedEvents = $state<Event[]>([]);
	let loading = $state(true);
	let error = $state<string | null>(null);
	let mapContainer: HTMLDivElement;

	function eventUrl(event: Event): string {
		const d = new Date(event.date);
		const dateParam = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
		return `/event?date=${dateParam}&title=${encodeURIComponent(event.title)}`;
	}

	function formatDate(dateStr: string): string {
		const d = new Date(dateStr);
		return d.toLocaleDateString('en-GB', { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' });
	}

	onMount(async () => {
		try {
			const L = await import('leaflet');
			await import('leaflet/dist/leaflet.css');

			// Fix Leaflet default icon paths broken by bundlers
			const iconRetinaUrl = (await import('leaflet/dist/images/marker-icon-2x.png')).default;
			const iconUrl = (await import('leaflet/dist/images/marker-icon.png')).default;
			const shadowUrl = (await import('leaflet/dist/images/marker-shadow.png')).default;
			L.Icon.Default.mergeOptions({ iconRetinaUrl, iconUrl, shadowUrl });

			// ~100km radius ≈ zoom 9 for Leaflet
			const DEFAULT_CENTER: [number, number] = [50.25, 5.5]; // Luxembourg, Belgium
			const NEARBY_ZOOM = 9;

			// Try to get user location first to center the map
			const userPos = await new Promise<{ lat: number; lng: number } | null>((resolve) => {
				if (!navigator.geolocation) return resolve(null);
				navigator.geolocation.getCurrentPosition(
					(pos) => resolve({ lat: pos.coords.latitude, lng: pos.coords.longitude }),
					() => resolve(null),
					{ timeout: 5000 }
				);
			});

			const center: [number, number] = userPos ? [userPos.lat, userPos.lng] : DEFAULT_CENTER;
			const map = L.map(mapContainer).setView(center, NEARBY_ZOOM);
			L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
				attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
			}).addTo(map);

			if (userPos) {
				L.marker([userPos.lat, userPos.lng], {
					icon: L.divIcon({
						className: 'bg-primary rounded-circle border border-white',
						iconSize: [14, 14]
					})
				})
					.addTo(map)
					.bindPopup('You are here');
			}

			const data = await events.load();
			allEvents = data;

			const mapped: Event[] = [];
			const unmapped: Event[] = [];
			const seen = new Map<string, { lat: number; lng: number } | null>();

			// Geocode unique postal codes
			const uniquePostalCodes = new Map<string, { postalCode: string; country?: string }>();
			for (const event of data) {
				if (event.postalCode) {
					const key = `${event.postalCode}:${event.country ?? ''}`;
					if (!uniquePostalCodes.has(key)) {
						uniquePostalCodes.set(key, { postalCode: event.postalCode, country: event.country ?? undefined });
					}
				}
			}

			for (const [key, { postalCode, country }] of uniquePostalCodes) {
				const coord = await cachedGeocode(postalCode, country);
				seen.set(key, coord ? { lat: coord.latitude, lng: coord.longitude } : null);
			}

			// Place markers
			for (const event of data) {
				if (!event.postalCode) {
					unmapped.push(event);
					continue;
				}
				const key = `${event.postalCode}:${event.country ?? ''}`;
				const coord = seen.get(key);
				if (!coord) {
					unmapped.push(event);
					continue;
				}
				mapped.push(event);
				const d = new Date(event.date);
				const dateStr = d.toLocaleDateString('en-GB', { weekday: 'short', day: 'numeric', month: 'short', year: 'numeric' });
				L.marker([coord.lat, coord.lng])
					.addTo(map)
					.bindPopup(`<strong><a href="${eventUrl(event)}">${event.title}</a></strong><br>${dateStr}<br>${event.town ?? ''}`);
			}

			unmappedEvents = unmapped;

			// Ensure tiles recalculate after view changes
			map.invalidateSize();

			loading = false;
		} catch (e: any) {
			error = e.message;
			loading = false;
		}
	});
</script>

<svelte:head>
	<title>FFBC Calendar — Map</title>
</svelte:head>

<h2 class="section-heading">Event Map</h2>

{#if loading}
	<div class="text-center py-5">
		<div class="spinner-border text-primary" role="status">
			<span class="visually-hidden">Loading...</span>
		</div>
	</div>
{:else if error}
	<div class="alert alert-danger">{error}</div>
{/if}

<div bind:this={mapContainer} id="map" class:d-none={error}></div>

{#if !loading && unmappedEvents.length > 0}
	<h4 class="mt-4 section-heading">Events Without Location</h4>
	<div class="table-responsive">
		<table class="table">
			<thead>
				<tr>
					<th>Date</th>
					<th>Event</th>
					<th>Details</th>
				</tr>
			</thead>
			<tbody>
				{#each unmappedEvents as event}
					<tr>
						<td>{formatDate(event.date)}</td>
						<td><a href={eventUrl(event)}>{event.title}</a></td>
						<td>{event.notes ?? ''}</td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>
{/if}
