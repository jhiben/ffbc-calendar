## Context

The FFBC application displays mountain biking events fetched from the FFBC website. Two views exist: a chronological list and a monthly calendar grid. Both rely on the `Event` model (Date, Title, Notes) served by `IEventStore`.

The remote parser (`FfbcWebEventStore`) already extracts a `placeText` value from the HTML (from the `calendar-row-place` div) but concatenates it into the `Notes` field alongside challenge information. The place text typically follows the Belgian format: `<PostalCode> <Town>` (e.g., "6900 Marche-en-Famenne").

The application is a server-rendered Razor Pages app using Bootstrap 5. There is no database — all data lives in memory with configurable cache TTL.

## Goals / Non-Goals

**Goals:**

- Extend the Event model with `Town` and `PostalCode` properties so location is structured data.
- Add a `/Map` Razor Page that renders an interactive map with event markers.
- Geocode events using Belgian postal codes to derive marker positions.
- Integrate smoothly with existing navigation and Bootstrap styling.

**Non-Goals:**

- Real-time location tracking or live updates.
- User-editable event locations.
- Driving directions or route planning.
- Supporting postal codes outside Belgium.
- Persisting geocoded coordinates in a database.

## Decisions

### 1. Map library: Leaflet.js with OpenStreetMap tiles

**Choice**: Use Leaflet.js loaded via CDN with OpenStreetMap tile layer.

**Alternatives considered**:
- **Google Maps**: Requires API key, usage fees, and terms-of-service compliance.
- **Mapbox**: Free tier exists but requires account and token management.
- **OpenLayers**: More powerful but heavier and more complex for this use case.

**Rationale**: Leaflet is lightweight (~40 KB), free, has no API key requirement, and pairs naturally with OpenStreetMap. Sufficient for displaying markers on a map of Belgium.

### 2. Geocoding approach: Server-side via Nominatim (OpenStreetMap)

**Choice**: Geocode postal codes server-side using the free OpenStreetMap Nominatim API (`https://nominatim.openstreetmap.org/search`), with results cached alongside events.

**Alternatives considered**:
- **Static postal code lookup table**: Deterministic but requires maintaining a dataset of ~1,100+ Belgian postal codes with coordinates.
- **Client-side geocoding**: Shifts load to browser, but creates visible latency as markers appear asynchronously.

**Rationale**: Nominatim is free for low-volume use (with proper User-Agent), the app already uses HTTP clients, and caching alongside events means geocoding only happens when the event cache refreshes. Belgian postal codes are coarse enough that postal-code-level accuracy is sufficient for a map view.

### 3. Event model extension: Add `Town` and `PostalCode` to `Event`

**Choice**: Add `string? Town` and `string? PostalCode` as nullable properties on the `Event` model.

**Rationale**: Nullable because in-memory seed events and parsing failures may not have location data. Existing views won't break — they don't reference these fields. The parser already extracts place text; it just needs to split it into structured fields.

### 4. Map page model: Server prepares JSON for client-side rendering

**Choice**: The `Map.cshtml.cs` page model fetches events, geocodes them, and serializes marker data (lat, lng, title, date, town) as a JSON array embedded in the page. Leaflet.js reads this client-side.

**Rationale**: Avoids a separate API endpoint. Keeps the pattern consistent with existing Razor Pages that fetch events in `OnGet`. The JSON payload is small (event count is typically low).

## Risks / Trade-offs

- **[Nominatim rate limits]** → Nominatim requires max 1 request/second and a descriptive User-Agent. Mitigation: batch-geocode only on cache miss and add a small delay between requests. The event count is typically low (tens, not thousands).
- **[Geocoding failures]** → Some postal codes may not resolve. Mitigation: Events without coordinates are omitted from the map but listed in a "could not be mapped" section below the map.
- **[CDN dependency for Leaflet]** → Map view won't work offline. Mitigation: Acceptable trade-off — the app already requires internet to fetch events from the FFBC website.
- **[Place text parsing fragility]** → The `<PostalCode> <Town>` format assumption may not hold for all events. Mitigation: Use a regex that matches a 4-digit Belgian postal code at the start of the string; fall back to treating the whole string as town if no match.
