## 1. Extend Event Model

- [x] 1.1 Add nullable `Town` and `PostalCode` string properties to `Event` model
- [x] 1.2 Update `FfbcWebEventStore.TryParseEvents` to parse place text into `PostalCode` and `Town` fields (regex: 4-digit code at start for Belgian postal codes; fall back to full text as Town)
- [x] 1.3 Update existing parser tests to verify `Town` and `PostalCode` extraction for known place text formats
- [x] 1.4 Add parser tests for edge cases: no place text, place text without postal code, place text with postal code

## 2. Geocoding Service

- [x] 2.1 Create `IGeocodingService` interface with a method to resolve postal code/town to latitude and longitude
- [x] 2.2 Implement `NominatimGeocodingService` that calls the OpenStreetMap Nominatim API with proper User-Agent and rate limiting
- [x] 2.3 Cache geocoding results in `IMemoryCache` so coordinates are not re-fetched on every request
- [x] 2.4 Register the geocoding service in `Program.cs`
- [x] 2.5 Add unit tests for geocoding service behavior (success, failure, caching)

## 3. Map Page

- [x] 3.1 Create `Map.cshtml.cs` page model that fetches events, geocodes them, and prepares a JSON array of marker data (lat, lng, title, date, town)
- [x] 3.2 Create `Map.cshtml` Razor view with a Leaflet.js map (CDN), rendering markers from the serialized JSON
- [x] 3.3 Implement marker popups showing event title, date, and town on click
- [x] 3.4 Display events without resolved coordinates in a list section below the map
- [x] 3.5 Handle the empty-events state with a centered-on-Belgium default view and a "no events" message
- [x] 3.6 Add unit tests for the Map page model

## 4. Navigation

- [x] 4.1 Add a "Map" link to the shared navigation bar in `_Layout.cshtml`

## 5. Verification

- [x] 5.1 Run all existing tests to confirm no regressions
- [x] 5.2 Manual smoke test: verify map loads, markers appear, popups work, and unmapped events are listed
