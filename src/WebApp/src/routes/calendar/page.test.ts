import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/svelte';
import { page } from '$app/state';

vi.mock('$lib/stores', () => ({
	events: {
		subscribe: vi.fn((fn: (value: never[]) => void) => {
			fn([]);
			return () => {};
		}),
		load: vi.fn(() => Promise.resolve([]))
	}
}));

import CalendarPage from './+page.svelte';

beforeEach(() => {
	page.url = new URL('http://localhost/calendar?year=2025&month=6');
});

describe('calendar/+page.svelte', () => {
	it('renders day headers (Mon through Sun)', async () => {
		render(CalendarPage);
		await waitFor(() => {
			expect(screen.getByText('Mon')).toBeInTheDocument();
		});
		for (const day of ['Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']) {
			expect(screen.getByText(day)).toBeInTheDocument();
		}
	});

	it('renders month name and year', () => {
		render(CalendarPage);
		expect(screen.getByText('June 2025')).toBeInTheDocument();
	});

	it('shows prev/next navigation buttons', () => {
		render(CalendarPage);
		const prevLink = screen.getByText(/Prev/);
		const nextLink = screen.getByText(/Next/);
		expect(prevLink).toBeInTheDocument();
		expect(nextLink).toBeInTheDocument();
		expect(prevLink.closest('a')).toHaveAttribute('href', '/calendar?year=2025&month=5');
		expect(nextLink.closest('a')).toHaveAttribute('href', '/calendar?year=2025&month=7');
	});
});
