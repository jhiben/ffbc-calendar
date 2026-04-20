<script lang="ts">
	import { page } from '$app/state';
	import { events } from '$lib/stores';
	import { fetchEventDetails } from '$lib/api';
	import type { Event, EventDetails } from '$lib/types';

	let event = $state<Event | null>(null);
	let details = $state<EventDetails | null>(null);
	let loading = $state(true);
	let notFound = $state(false);

	const dateParam = $derived(page.url.searchParams.get('date'));
	const titleParam = $derived(page.url.searchParams.get('title'));

	$effect(() => {
		loadEvent();
	});

	async function loadEvent() {
		loading = true;
		notFound = false;
		event = null;
		details = null;

		if (!dateParam || !titleParam) {
			notFound = true;
			loading = false;
			return;
		}

		const data = await events.load();
		const targetDate = new Date(dateParam);
		if (isNaN(targetDate.getTime())) {
			notFound = true;
			loading = false;
			return;
		}

		const found = data.find((e) => {
			const d = new Date(e.date);
			return (
				d.getFullYear() === targetDate.getFullYear() &&
				d.getMonth() === targetDate.getMonth() &&
				d.getDate() === targetDate.getDate() &&
				e.title.toLowerCase() === titleParam.toLowerCase()
			);
		});

		if (!found) {
			notFound = true;
			loading = false;
			return;
		}

		event = found;

		if (found.eventId) {
			try {
				details = await fetchEventDetails(found.eventId);
			} catch {
				// Details are optional
			}
		}

		loading = false;
	}

	function formatDate(dateStr: string): string {
		const d = new Date(dateStr);
		return d.toLocaleDateString('en-GB', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
	}

	function difficultyClass(difficulty: string | null): string {
		if (!difficulty) return 'difficulty-unknown';
		const lower = difficulty.toLowerCase().trim();
		if (lower === 'easy' || lower === 'facile') return 'difficulty-easy';
		if (lower === 'moderate' || lower === 'modéré' || lower === 'moyen') return 'difficulty-moderate';
		if (lower === 'hard' || lower === 'difficile') return 'difficulty-hard';
		if (lower === 'expert' || lower === 'très difficile') return 'difficulty-expert';
		return 'difficulty-unknown';
	}
</script>

<svelte:head>
	<title>FFBC Calendar — {event?.title ?? 'Event Detail'}</title>
</svelte:head>

{#if loading}
	<div class="text-center py-5">
		<div class="spinner-border text-primary" role="status">
			<span class="visually-hidden">Loading...</span>
		</div>
	</div>
{:else if notFound}
	<div class="alert alert-warning">
		<h4>Event Not Found</h4>
		<p>The requested event could not be found.</p>
		<a href="/list" class="btn btn-primary">Back to Events</a>
	</div>
{:else if event}
	<h2 class="section-heading">{event.title}</h2>

	<div class="card detail-section mb-3">
		<div class="card-header">📋 General Information</div>
		<div class="card-body">
			<p><span class="detail-emoji">📅</span> <strong>Date:</strong> {formatDate(event.date)}</p>
			{#if event.town}
				<p><span class="detail-emoji">📍</span> <strong>Location:</strong> {event.town}{event.postalCode ? ` (${event.postalCode})` : ''}{event.province ? ` — ${event.province}` : ''}</p>
			{/if}
			{#if event.country}
				<p><span class="detail-emoji">🌍</span> <strong>Country:</strong> {event.country}</p>
			{/if}
			{#if event.challenge}
				<p><span class="detail-emoji">🏆</span> <strong>Challenge:</strong> {event.challenge}</p>
			{/if}
			{#if event.notes}
				<p><span class="detail-emoji">📝</span> <strong>Notes:</strong> {event.notes}</p>
			{/if}
		</div>
	</div>

	{#if details}
		<div class="card detail-section mb-3">
			<div class="card-header">📍 Additional Details</div>
			<div class="card-body">
				{#if details.startLocation}
					<p><span class="detail-emoji">🏁</span> <strong>Start:</strong> {details.startLocation}</p>
				{/if}
				{#if details.address}
					<p>
						<span class="detail-emoji">📫</span> <strong>Address:</strong>
						{#if details.mapsUrl}
							<a href={details.mapsUrl} target="_blank" rel="noopener">{details.address}</a>
						{:else}
							{details.address}
						{/if}
					</p>
				{/if}
				{#if details.club}
					<p><span class="detail-emoji">🏢</span> <strong>Club:</strong> {details.club}</p>
				{/if}
				{#if details.registrationUrl}
					<p><a href={details.registrationUrl} target="_blank" rel="noopener" class="btn btn-primary btn-sm">Register</a></p>
				{/if}
				{#if details.website}
					<p><span class="detail-emoji">🌐</span> <strong>Website:</strong> <a href={details.website} target="_blank" rel="noopener">{details.website}</a></p>
				{/if}
				{#if details.notes}
					<p><span class="detail-emoji">📝</span> <strong>Notes:</strong> {details.notes}</p>
				{/if}
			</div>
		</div>

		{#if details.activities.length > 0}
			<div class="card detail-section mb-3">
				<div class="card-header">🚴 Activities</div>
				<div class="activity-cards">
					{#each details.activities as activity}
						<div class="activity-card">
							<div class="activity-card-header">
								<span class="activity-type-badge">{activity.type ?? 'Activity'}</span>
							</div>
							<div class="activity-metrics">
								{#if activity.distance}
									<span class="activity-metric">
										<span class="activity-metric-emoji">📏</span>
										<span class="activity-metric-value">{activity.distance}</span>
									</span>
								{/if}
								{#if activity.elevation}
									<span class="activity-metric">
										<span class="activity-metric-emoji">⛰️</span>
										<span class="activity-metric-value" class:muted={activity.elevation === 'N.A'}>{activity.elevation}</span>
									</span>
								{/if}
								{#if activity.difficulty}
									<span class="activity-metric {difficultyClass(activity.difficulty)}">
										<span class="activity-metric-emoji">💪</span>
										<span class="activity-metric-value">{activity.difficulty}</span>
									</span>
								{/if}
								{#if activity.time}
									<span class="activity-metric">
										<span class="activity-metric-emoji">🕐</span>
										<span class="activity-metric-value">{activity.time}</span>
									</span>
								{/if}
								{#if activity.ravito}
									<span class="activity-metric">
										<span class="activity-metric-emoji">🥤</span>
										<span class="activity-metric-value" class:muted={activity.ravito === 'N.A'}>{activity.ravito}</span>
									</span>
								{/if}
								{#if activity.price}
									<span class="activity-metric price-paid">
										<span class="activity-metric-emoji">💰</span>
										<span class="activity-metric-value">{activity.price}</span>
									</span>
								{/if}
							</div>
						</div>
					{/each}
				</div>
			</div>
		{/if}
	{/if}

	<div class="mt-3">
		<a href="/list" class="btn btn-outline-secondary">← Back to Events</a>
	</div>
{/if}
