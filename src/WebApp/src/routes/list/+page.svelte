<script lang="ts">
	import { events } from '$lib/stores';
	import type { Event } from '$lib/types';

	let items = $state<Event[]>([]);
	let loading = $state(true);
	let error = $state<string | null>(null);

	$effect(() => {
		events.load().then((data) => {
			items = data;
			loading = false;
		}).catch((e) => {
			error = e.message;
			loading = false;
		});
	});

	function formatDate(dateStr: string): string {
		const d = new Date(dateStr);
		return d.toLocaleDateString('en-GB', { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' });
	}

	function eventUrl(event: Event): string {
		const d = new Date(event.date);
		const dateParam = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
		return `/event?date=${dateParam}&title=${encodeURIComponent(event.title)}`;
	}
</script>

<svelte:head>
	<title>FFBC Calendar — Events List</title>
</svelte:head>

<h2 class="section-heading">Events</h2>

{#if loading}
	<div class="text-center py-5">
		<div class="spinner-border text-primary" role="status">
			<span class="visually-hidden">Loading...</span>
		</div>
	</div>
{:else if error}
	<div class="alert alert-danger">{error}</div>
{:else if items.length === 0}
	<div class="alert alert-info">No events found.</div>
{:else}
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
				{#each items as event}
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
