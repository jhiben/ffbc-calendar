<script lang="ts">
	import { page } from '$app/state';
	import { events } from '$lib/stores';
	import type { Event } from '$lib/types';

	let allEvents = $state<Event[]>([]);
	let loading = $state(true);
	let error = $state<string | null>(null);

	const searchParams = $derived(page.url.searchParams);
	const displayYear = $derived(Number(searchParams.get('year')) || new Date().getFullYear());
	const displayMonth = $derived(Number(searchParams.get('month')) || new Date().getMonth() + 1);

	const monthName = $derived(new Date(displayYear, displayMonth - 1).toLocaleString('en-GB', { month: 'long' }));

	const prevMonth = $derived(displayMonth === 1 ? 12 : displayMonth - 1);
	const prevYear = $derived(displayMonth === 1 ? displayYear - 1 : displayYear);
	const nextMonth = $derived(displayMonth === 12 ? 1 : displayMonth + 1);
	const nextYear = $derived(displayMonth === 12 ? displayYear + 1 : displayYear);

	const today = new Date();
	const todayYear = today.getFullYear();
	const todayMonth = today.getMonth() + 1;
	const todayDay = today.getDate();

	interface DayCell {
		date: Date | null;
		day: number | null;
		events: Event[];
		isToday: boolean;
	}

	const weeks = $derived.by(() => {
		const firstDay = new Date(displayYear, displayMonth - 1, 1);
		const daysInMonth = new Date(displayYear, displayMonth, 0).getDate();
		// Monday = 0, Sunday = 6
		let startDow = (firstDay.getDay() + 6) % 7;

		const monthEvents = allEvents.filter((e) => {
			const d = new Date(e.date);
			return d.getFullYear() === displayYear && d.getMonth() + 1 === displayMonth;
		});

		const rows: DayCell[][] = [];
		let currentDay = 1;

		while (currentDay <= daysInMonth) {
			const week: DayCell[] = [];
			for (let col = 0; col < 7; col++) {
				if ((rows.length === 0 && col < startDow) || currentDay > daysInMonth) {
					week.push({ date: null, day: null, events: [], isToday: false });
				} else {
					const day = currentDay;
					const cellDate = new Date(displayYear, displayMonth - 1, day);
					const dayEvents = monthEvents.filter((e) => new Date(e.date).getDate() === day);
					const isToday = displayYear === todayYear && displayMonth === todayMonth && day === todayDay;
					week.push({ date: cellDate, day, events: dayEvents, isToday });
					currentDay++;
				}
			}
			rows.push(week);
		}
		return rows;
	});

	function eventUrl(event: Event): string {
		const d = new Date(event.date);
		const dateParam = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
		return `/event?date=${dateParam}&title=${encodeURIComponent(event.title)}`;
	}

	$effect(() => {
		events.load().then((data) => {
			allEvents = data;
			loading = false;
		}).catch((e) => {
			error = e.message;
			loading = false;
		});
	});

	const dayHeaders = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
</script>

<svelte:head>
	<title>FFBC Calendar — {monthName} {displayYear}</title>
</svelte:head>

<div class="d-flex justify-content-between align-items-center mb-3">
	<a href="/calendar?year={prevYear}&month={prevMonth}" class="btn btn-outline-secondary">&laquo; Prev</a>
	<h2 class="mb-0">{monthName} {displayYear}</h2>
	<a href="/calendar?year={nextYear}&month={nextMonth}" class="btn btn-outline-secondary">Next &raquo;</a>
</div>

{#if loading}
	<div class="text-center py-5">
		<div class="spinner-border text-primary" role="status">
			<span class="visually-hidden">Loading...</span>
		</div>
	</div>
{:else if error}
	<div class="alert alert-danger">{error}</div>
{:else}
	<table class="calendar-table">
		<thead>
			<tr>
				{#each dayHeaders as day}
					<th>{day}</th>
				{/each}
			</tr>
		</thead>
		<tbody>
			{#each weeks as week}
				<tr>
					{#each week as cell}
						<td class:calendar-empty={!cell.date} class:calendar-today={cell.isToday}>
							{#if cell.day}
								<div class="calendar-day-number">{cell.day}</div>
								{#each cell.events as event}
									<a href={eventUrl(event)} class="event-chip" title={event.title}>{event.title}</a>
								{/each}
							{/if}
						</td>
					{/each}
				</tr>
			{/each}
		</tbody>
	</table>
{/if}
