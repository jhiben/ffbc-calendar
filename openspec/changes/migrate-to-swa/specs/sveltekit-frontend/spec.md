## ADDED Requirements

### Requirement: SvelteKit project with static adapter
The frontend SHALL be a SvelteKit project configured with `@sveltejs/adapter-static` that produces a fully static build output suitable for Azure Static Web Apps.

#### Scenario: Static build produces HTML/CSS/JS output
- **WHEN** `npm run build` is executed in the frontend project
- **THEN** the build SHALL produce a static output directory containing HTML, CSS, and JavaScript files with no server-side runtime dependency

#### Scenario: SPA fallback routing configured
- **WHEN** the static adapter is configured
- **THEN** it SHALL generate a fallback `index.html` so that all client-side routes resolve correctly on page refresh

### Requirement: File-based routing matches current pages
The SvelteKit project SHALL use file-based routing to provide routes equivalent to the current Razor Pages: index (`/`), list (`/list`), calendar (`/calendar`), map (`/map`), and event detail (`/event`).

#### Scenario: Index route renders landing page
- **WHEN** the user navigates to `/`
- **THEN** the app SHALL render the landing page with hero section and navigation cards to List, Calendar, and Map views

#### Scenario: List route renders event list
- **WHEN** the user navigates to `/list`
- **THEN** the app SHALL fetch events from `/api/events` and render them in a chronological table

#### Scenario: Calendar route renders month grid
- **WHEN** the user navigates to `/calendar`
- **THEN** the app SHALL fetch events from `/api/events` and render a monthly calendar grid with event badges

#### Scenario: Map route renders interactive map
- **WHEN** the user navigates to `/map`
- **THEN** the app SHALL fetch events and geocoding data and render an interactive Leaflet map with event markers

#### Scenario: Event detail route renders event page
- **WHEN** the user navigates to `/event?date=YYYY-MM-DD&title=...`
- **THEN** the app SHALL fetch event details from the API and render the full event detail page

### Requirement: Shared layout with navigation
The app SHALL provide a shared layout component with a navigation bar, footer, and content area that mirrors the current Razor Pages layout.

#### Scenario: Navigation bar present on all pages
- **WHEN** any page is rendered
- **THEN** a navigation bar SHALL be displayed with links to List, Calendar, Map, and a brand logo

#### Scenario: Active link highlighted
- **WHEN** the user is on a specific page
- **THEN** the corresponding navigation link SHALL be visually highlighted as active

#### Scenario: Footer present on all pages
- **WHEN** any page is rendered
- **THEN** a footer SHALL be displayed with copyright, privacy link, and RSS feed link

### Requirement: API client module
The frontend SHALL include a shared API client module that centralizes all fetch calls to the Azure Functions backend.

#### Scenario: API client fetches events
- **WHEN** any page needs event data
- **THEN** it SHALL call the API client which sends a GET request to `/api/events` and returns typed event objects

#### Scenario: API client handles errors gracefully
- **WHEN** an API request fails (network error, non-200 status)
- **THEN** the API client SHALL return an empty result and the page SHALL display a user-friendly message

#### Scenario: API responses cached in Svelte stores
- **WHEN** events are fetched successfully
- **THEN** the API client SHALL cache the response in a Svelte writable store so subsequent navigations reuse cached data without re-fetching

### Requirement: Design system ported to SvelteKit
The existing CSS design system (custom properties, Bootstrap overrides, component styles) SHALL be ported to the SvelteKit project to maintain visual parity.

#### Scenario: CSS custom properties available globally
- **WHEN** any page is rendered
- **THEN** all existing CSS custom properties (`--color-primary`, `--color-accent`, etc.) SHALL be defined and used

#### Scenario: Bootstrap loaded via npm
- **WHEN** the frontend project is built
- **THEN** Bootstrap 5 CSS SHALL be loaded via npm package instead of local wwwroot copy

#### Scenario: Visual appearance matches current app
- **WHEN** any page is rendered in the new frontend
- **THEN** the visual appearance (colors, spacing, typography, component styles) SHALL match the current Razor Pages app
