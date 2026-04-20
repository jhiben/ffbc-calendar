## MODIFIED Requirements

### Requirement: Display events on an interactive map
The system SHALL display events as markers on an interactive map using Leaflet.js with OpenStreetMap tiles. Event data and geocoding coordinates SHALL be fetched from the API rather than prepared server-side.

#### Scenario: Events with location data displayed as markers
- **WHEN** the user navigates to the Map page
- **THEN** the app SHALL fetch events from `/api/events`, geocode each event via `/api/geocode/{postalCode}`, and display markers on the map

#### Scenario: Map centered on user location with 50km radius
- **WHEN** the Map page loads and the user grants geolocation permission
- **THEN** the map SHALL center on the user's current coordinates at zoom level 10 (approximately 50km radius view)

#### Scenario: Map falls back to Belgium center when geolocation unavailable
- **WHEN** the Map page loads and geolocation is denied, unavailable, or times out
- **THEN** the map SHALL center on Belgium and auto-fit its bounds to show all markers

#### Scenario: No events available
- **WHEN** the user navigates to the Map page and the API returns no events
- **THEN** the map SHALL display centered on the user's location (if available) or Belgium at a default zoom level with a message indicating no events are available

### Requirement: Show event details on marker interaction
The system SHALL display event information in a popup when the user interacts with a map marker. The popup SHALL include a link to the Event Detail page.

#### Scenario: Marker popup shows event details
- **WHEN** the user clicks on a map marker
- **THEN** a popup SHALL display the event title, date, and town

#### Scenario: Marker popup links to detail page
- **WHEN** the user clicks on a map marker
- **THEN** the popup SHALL include a hyperlink to the Event Detail page for that event

### Requirement: Events without location are listed separately
Events that cannot be geocoded SHALL NOT be silently hidden. Event titles in the unmapped list SHALL link to the Event Detail page.

#### Scenario: Ungeocodable events shown below the map
- **WHEN** one or more events do not have a resolved location (geocoding returned 404 or event lacks postal code)
- **THEN** those events SHALL be listed in a section below the map with their title and date

#### Scenario: Unmapped event titles link to detail page
- **WHEN** the unmapped events table is rendered
- **THEN** each event title SHALL be a hyperlink that navigates to the Event Detail page for that event

### Requirement: Geocoding performed client-side via API
The map page SHALL geocode events by calling the `/api/geocode/{postalCode}` endpoint for each unique postal code, rather than relying on server-side geocoding during page rendering.

#### Scenario: Geocoding requests batched by unique postal code
- **WHEN** the map page loads with events
- **THEN** it SHALL send one geocode API request per unique postal code (not per event)

#### Scenario: Geocoding results cached in client store
- **WHEN** geocoding results are fetched
- **THEN** they SHALL be cached in a Svelte store so subsequent map page visits do not re-fetch
