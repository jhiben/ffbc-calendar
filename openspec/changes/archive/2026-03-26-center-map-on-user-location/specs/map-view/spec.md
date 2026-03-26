## MODIFIED Requirements

### Requirement: Display events on an interactive map

The system SHALL display events as markers on an interactive map using Leaflet.js with OpenStreetMap tiles. The map SHALL be centered on the user's current location with a 50km radius view when geolocation is available; otherwise, it SHALL fall back to being centered on Belgium and zoomed to fit all visible markers.

#### Scenario: Events with location data displayed as markers

- **WHEN** the user navigates to the Map page and events have valid postal code or town data
- **THEN** each event with a resolved location SHALL appear as a marker on the map

#### Scenario: Map centered on user location with 50km radius

- **WHEN** the Map page loads and the user grants geolocation permission
- **THEN** the map SHALL center on the user's current coordinates at zoom level 10 (approximately 50km radius view)

#### Scenario: Map falls back to Belgium center when geolocation unavailable

- **WHEN** the Map page loads and geolocation is denied, unavailable, or times out
- **THEN** the map SHALL center on Belgium and auto-fit its bounds to show all markers (existing behavior)

#### Scenario: No events available

- **WHEN** the user navigates to the Map page and no events exist
- **THEN** the map SHALL display centered on the user's location (if available) or Belgium at a default zoom level with a message indicating no events are available
