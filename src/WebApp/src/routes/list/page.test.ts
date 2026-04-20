import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/svelte';
import type { Event } from '$lib/types';
import { events } from '$lib/stores';

vi.mock('$lib/stores', () => {
	const load = vi.fn(() => Promise.resolve([]));
	return {
		events: {
			subscribe: vi.fn((fn: (value: Event[]) => void) => {
				fn([]);
				return () => {};
			}),
			load
		}
	};
});

const testEvents: Event[] = [
	{
		date: '2025-03-15T00:00:00',
		title: 'Test MTB Event',
		notes: 'A fun ride',
		town: 'Brussels',
		postalCode: '1000',
		country: 'Belgium',
		province: 'Brussels',
		challenge: null,
		eventId: 'evt-1'
	},
	{
		date: '2025-04-20T00:00:00',
		title: 'Another Event',
		notes: null,
		town: 'Gent',
		postalCode: '9000',
		country: 'Belgium',
		province: 'East Flanders',
		challenge: 'Challenge X',
		eventId: 'evt-2'
	}
];

import ListPage from './+page.svelte';

describe('list/+page.svelte', () => {
	beforeEach(() => {
		vi.mocked(events.load).mockReset();
	});

	it('shows loading spinner initially', () => {
		vi.mocked(events.load).mockReturnValue(new Promise(() => {}));
		render(ListPage);
		expect(screen.getByRole('status')).toBeInTheDocument();
	});

	it('renders events in a table when API returns data', async () => {
		vi.mocked(events.load).mockResolvedValue(testEvents);
		render(ListPage);
		await waitFor(() => {
			expect(screen.getByText('Test MTB Event')).toBeInTheDocument();
		});
		expect(screen.getByText('Another Event')).toBeInTheDocument();
		expect(screen.getByText('A fun ride')).toBeInTheDocument();
	});

	it('shows "No events found" when API returns empty array', async () => {
		vi.mocked(events.load).mockResolvedValue([]);
		render(ListPage);
		await waitFor(() => {
			expect(screen.getByText('No events found.')).toBeInTheDocument();
		});
	});
});
