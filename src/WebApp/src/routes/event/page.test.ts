import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/svelte';
import { page } from '$app/state';
import { events } from '$lib/stores';

vi.mock('$lib/stores', () => ({
	events: {
		subscribe: vi.fn((fn: (value: never[]) => void) => {
			fn([]);
			return () => {};
		}),
		load: vi.fn(() => Promise.resolve([]))
	}
}));

vi.mock('$lib/api', () => ({
	fetchEventDetails: vi.fn(() => Promise.resolve(null))
}));

import EventPage from './+page.svelte';

describe('event/+page.svelte', () => {
	beforeEach(() => {
		vi.mocked(events.load).mockReset();
		page.url = new URL('http://localhost/event');
	});

	it('shows "Event Not Found" when no query params', async () => {
		vi.mocked(events.load).mockResolvedValue([]);
		render(EventPage);
		await waitFor(() => {
			expect(screen.getByText('Event Not Found')).toBeInTheDocument();
		});
	});

	it('shows loading spinner when loading', () => {
		vi.mocked(events.load).mockReturnValue(new Promise(() => {}));
		page.url = new URL('http://localhost/event?date=2025-03-15&title=Test');
		render(EventPage);
		expect(screen.getByRole('status')).toBeInTheDocument();
	});
});
