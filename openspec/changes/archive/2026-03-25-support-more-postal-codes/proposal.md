## Why

The FFBC event source includes events from both Belgium and France. The current postal code parser only recognizes bare 4-digit Belgian codes (e.g., "6780"). Place text may also arrive as "B-1234 Town" (Belgian with prefix) or "F-12345 Town" (French with 5-digit code). These formats are currently unrecognized, causing events to lose their location data and fall into the "unmapped" list on the map view.

## What Changes

- **Extend `ParsePlace`** to recognize four postal code formats: bare 4-digit (`1234`), Belgian-prefixed (`B-1234`), French-prefixed (`F-12345`), and bare 5-digit (`59000`).
- **Add a `Country` property** to the `Event` model so the geocoder knows which country to query.
- **Update `NominatimGeocodingService`** to pass the correct country (Belgium or France) instead of hardcoding Belgium.
- **Update existing tests** for the new formats and country derivation.

## Non-goals

- Supporting postal codes from countries other than Belgium and France.
- Changing how the map view displays events (no UI changes).
- Modifying the remote event store fetch logic or HTML scraping.

## Capabilities

### New Capabilities

_None._

### Modified Capabilities

- `map-view`: The "Event model includes town and postal code" requirement changes to include a Country field. The "Geocode events by postal code" requirement changes to pass the correct country to Nominatim.

## Impact

- **Models**: `Event.cs` gains a `Country` nullable property.
- **Services**: `FfbcWebEventStore.ParsePlace` regex updated; outputs country. `NominatimGeocodingService.GeocodeAsync` accepts and uses country.
- **Interface**: `IGeocodingService` signature gains a country parameter.
- **Pages**: `Map.cshtml.cs` passes country through to geocoder.
- **Tests**: Parser tests and geocoding tests updated for new formats.
