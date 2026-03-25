## Why

The application currently offers calendar and list views for mountain biking events, but riders often need to assess event proximity at a glance — especially when planning weekends or multi-day trips. A map view lets users visually locate events geographically, making it easy to find nearby rides and plan routes. The remote data source already provides place information that is currently buried in the Notes field; surfacing it as structured location data enables map placement.

## What Changes

- **Add `Town` and `PostalCode` properties** to the `Event` model so location data is first-class rather than embedded in Notes.
- **Update the event parser** in `FfbcWebEventStore` to extract town and postal code from the place text into the new fields.
- **Add a Map View page** (`/Map`) that displays events as pins on an interactive map using Leaflet.js with OpenStreetMap tiles (free, no API key required).
- **Geocode events by postal code** to obtain latitude/longitude for map placement (using a static lookup or a free geocoding approach for Belgian postal codes).
- **Add navigation** to the map view in the shared layout nav bar.

## Non-goals

- Real-time event tracking or live location updates.
- User-submitted locations or editable event positions.
- Driving directions or route planning between events.
- Supporting non-Belgian postal codes or international events.

## Capabilities

### New Capabilities

- `map-view`: Interactive map page displaying events as markers positioned by town/postal code, with event details shown on marker interaction.

### Modified Capabilities

_None — existing calendar-view, list-view, and remote-event-store specs do not change at the requirements level. The Event model extension is an implementation detail that does not alter existing view contracts._

## Impact

- **Models**: `Event.cs` gains `Town` and `PostalCode` properties.
- **Services**: `FfbcWebEventStore.TryParseEvents` updated to populate new fields instead of concatenating place into Notes.
- **Pages**: New `Map.cshtml` / `Map.cshtml.cs` Razor Page added.
- **Layout**: `_Layout.cshtml` gains a "Map" nav link.
- **Client-side**: Leaflet.js library added to `wwwroot/lib/` or loaded via CDN.
- **Tests**: New tests for the map page model; updated parser tests to verify Town/PostalCode extraction.
