## ADDED Requirements

### Requirement: Events API endpoint
The Functions app SHALL expose a `GET /api/events` endpoint that returns all mountain biking events as a JSON array.

#### Scenario: Successful events fetch
- **WHEN** a GET request is sent to `/api/events`
- **THEN** the response SHALL have status 200 and a JSON body containing an array of event objects with fields: eventId, date, title, notes, town, postalCode, country, province, challenge

#### Scenario: Empty events list
- **WHEN** the upstream FFBC API returns no events or fails
- **THEN** the endpoint SHALL return status 200 with an empty JSON array

#### Scenario: Events ordered by date ascending
- **WHEN** events are returned
- **THEN** they SHALL be ordered by date ascending

### Requirement: Event details API endpoint
The Functions app SHALL expose a `GET /api/events/{eventId}/details` endpoint that returns enriched details for a single event.

#### Scenario: Successful details fetch
- **WHEN** a GET request is sent to `/api/events/{eventId}/details` with a valid event ID
- **THEN** the response SHALL have status 200 and a JSON body containing enriched event details: registrationUrl, startLocation, address, mapsUrl, club, activities, notes, website

#### Scenario: Event ID not found
- **WHEN** a GET request is sent with an event ID that does not exist or the upstream API returns no data
- **THEN** the endpoint SHALL return status 404

#### Scenario: Activity objects include all metrics
- **WHEN** enriched details contain activities
- **THEN** each activity object SHALL include: type, distance, elevation, difficulty, time, ravito, price

### Requirement: Geocode API endpoint
The Functions app SHALL expose a `GET /api/geocode/{postalCode}` endpoint that returns geographic coordinates for a postal code.

#### Scenario: Successful geocoding
- **WHEN** a GET request is sent to `/api/geocode/{postalCode}` with a valid postal code
- **THEN** the response SHALL have status 200 and a JSON body containing latitude and longitude

#### Scenario: Optional country parameter
- **WHEN** a GET request includes a `country` query parameter
- **THEN** the geocoding service SHALL use that country for the Nominatim lookup

#### Scenario: Geocoding failure
- **WHEN** the Nominatim API fails or returns no results
- **THEN** the endpoint SHALL return status 404

### Requirement: RSS feed API endpoint
The Functions app SHALL expose a `GET /api/feed.xml` endpoint that returns a valid RSS 2.0 feed.

#### Scenario: Feed returns valid RSS 2.0
- **WHEN** a GET request is sent to `/api/feed.xml`
- **THEN** the response SHALL have status 200, content type `application/rss+xml`, and a valid RSS 2.0 XML body

#### Scenario: Feed items map to events
- **WHEN** events exist
- **THEN** each event SHALL be an `<item>` with title, link, description, and pubDate

### Requirement: Dependency injection and configuration
The Functions app SHALL use .NET dependency injection to register services and read configuration from environment variables or `local.settings.json`.

#### Scenario: Services registered via DI
- **WHEN** the Functions host starts
- **THEN** `IEventStore`, `IEventDetailsService`, and `IGeocodingService` SHALL be registered as singleton services

#### Scenario: Configuration read from settings
- **WHEN** the Functions host starts
- **THEN** FFBC event store options SHALL be read from configuration (environment variables in production, `local.settings.json` in development)

### Requirement: CORS configured for SWA
The Functions app SHALL allow cross-origin requests from the Static Web App frontend during local development.

#### Scenario: Local development CORS
- **WHEN** the frontend dev server makes API requests during local development
- **THEN** the Functions app SHALL accept requests from `http://localhost:5173` (SvelteKit dev server default)

#### Scenario: Production CORS via SWA
- **WHEN** the app is deployed to Azure Static Web Apps
- **THEN** CORS SHALL be handled by the SWA reverse proxy (no explicit CORS config needed in Functions)
