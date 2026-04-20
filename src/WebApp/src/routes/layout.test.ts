import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/svelte';
import { page } from '$app/state';
import { createRawSnippet } from 'svelte';

import Layout from './+layout.svelte';

beforeEach(() => {
	page.url = new URL('http://localhost/list');
});

const children = createRawSnippet(() => ({
	render: () => '<div data-testid="child">child content</div>'
}));

describe('+layout.svelte', () => {
	it('renders the navbar brand "FFBC Calendar"', () => {
		render(Layout, { props: { children } });
		expect(screen.getByText('FFBC Calendar')).toBeInTheDocument();
	});

	it('renders nav links for List, Calendar, Map', () => {
		render(Layout, { props: { children } });
		expect(screen.getByText('List')).toBeInTheDocument();
		expect(screen.getByText('Calendar')).toBeInTheDocument();
		expect(screen.getByText('Map')).toBeInTheDocument();
	});

	it('renders the footer with RSS feed link', () => {
		render(Layout, { props: { children } });
		const rssLink = screen.getByText('RSS Feed');
		expect(rssLink).toBeInTheDocument();
		expect(rssLink.closest('a')).toHaveAttribute('href', '/feed.xml');
	});
});
