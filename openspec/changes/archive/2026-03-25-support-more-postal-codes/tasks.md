## 1. Event Model

- [x] 1.1 Add nullable `Country` string property to the `Event` model

## 2. Parser Update

- [x] 2.1 Replace `BelgianPostalCodeRegex` with a new regex that matches bare 4-digit, `B-` prefixed 4-digit, and `F-` prefixed 5-digit postal codes
- [x] 2.2 Update `ParsePlace` to output `country` in addition to `town` and `postalCode`, deriving country from the prefix (`F` → France, `B` or absent → Belgium)
- [x] 2.3 Update `TryParseEvents` to populate `Event.Country` from the parser output
- [x] 2.4 Update existing parser tests to verify `Country` is set to `"Belgium"` for known Belgian formats
- [x] 2.5 Add parser tests for `B-` prefixed format (e.g., `"B-1234 Town"`)
- [x] 2.6 Add parser tests for `F-` prefixed format (e.g., `"F-12345 Town"`)
- [x] 2.7 Add parser test for bare 5-digit French format (e.g., `"59000 Lille"`)

## 3. Geocoding Service

- [x] 3.1 Update `IGeocodingService.GeocodeAsync` signature to accept a `country` parameter
- [x] 3.2 Update `NominatimGeocodingService` to use the provided country in the Nominatim query, defaulting to Belgium when null
- [x] 3.3 Update `Map.cshtml.cs` to pass `Event.Country` to the geocoding service
- [x] 3.4 Update geocoding tests for country-aware queries

## 4. Verification

- [x] 4.1 Run all tests to confirm no regressions
