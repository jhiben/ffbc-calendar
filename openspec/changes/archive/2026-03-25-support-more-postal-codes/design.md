## Context

The `FfbcWebEventStore.ParsePlace` method currently uses a single regex (`^(?:.*\s-\s)?(\d{4})\s+(.+)$`) targeting bare 4-digit Belgian postal codes. The FFBC event source can return place text in three formats:

1. `"Luxembourg - 6780 MESSANCY"` or `"6780 MESSANCY"` — bare Belgian (currently supported)
2. `"B-1234 Town"` — Belgian with country prefix
3. `"F-12345 Town"` — French with country prefix and 5-digit code
4. `"59000 Town"` — bare French with 5-digit code and no prefix

The geocoder (`NominatimGeocodingService`) hardcodes `country=Belgium` in its Nominatim query, which fails for French postal codes.

## Goals / Non-Goals

**Goals:**

- Parse all three postal code formats, extracting postal code, town, and country.
- Also support bare 5-digit French postal codes without a prefix.
- Pass the correct country to the Nominatim geocoding API.
- Add a `Country` property to `Event` for downstream use.

**Non-Goals:**

- Supporting countries beyond Belgium and France.
- Changing UI/display of events.
- Altering the HTML scraping or fetch logic.

## Decisions

### 1. Single regex with alternation for all formats

**Choice**: Replace the current regex with one that handles all three formats in a single pattern using alternation.

Pattern: `^(?:.*\s-\s)?(?:([BF])-)?(\d{4,5})\s+(.+)$`

- Optional province prefix (`Luxembourg - `) already handled
- Optional country prefix `B-` or `F-` captured in group 1
- Postal code (4 or 5 digits) captured in group 2
- Town captured in group 3
- Country derived: `F` → `"France"`, `B` or absent with 4 digits → `"Belgium"`, absent with 5 digits → `"France"`

**Alternatives considered**:
- **Multiple regexes tried in sequence**: More readable but slower and harder to maintain.
- **Enum for country**: Overkill — a nullable string suffices for two countries.

**Rationale**: Minimal change to existing code, single-pass matching, easy to extend later.

### 2. Add `Country` as nullable string on `Event`

**Choice**: Add `string? Country` to the `Event` model. Values: `"Belgium"`, `"France"`, or `null`.

**Rationale**: A nullable string is simple and sufficient. An enum would add ceremony for only two values. Null means country couldn't be determined (same as no postal code).

### 3. Update `IGeocodingService` signature to accept country

**Choice**: Change `GeocodeAsync(string postalCode, string? town)` to `GeocodeAsync(string postalCode, string? town, string? country)`.

**Rationale**: The geocoder needs to know which country to pass to Nominatim. Defaulting to Belgium when country is null preserves backward compatibility.

## Risks / Trade-offs

- **[Regex complexity]** → The combined regex is slightly harder to read. Mitigation: add a comment explaining the format groups.
- **[Breaking interface change]** → `IGeocodingService.GeocodeAsync` signature changes. Mitigation: only one implementation and one caller exist; update both together.
- **[Unknown postal code formats]** → Future formats may not match. Mitigation: unmatched text still falls through to `town = placeText, postalCode = null` — same graceful degradation as before.
