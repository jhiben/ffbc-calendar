## Context

The application currently fetches event listings from the FFBC calendar API (`ajax_request_calendrier.php`) which provides basic event information. The FFBC website also has a popup API (`ajax_request_popup_calendar.php`) that returns detailed event information when given an event ID.

**Current state:**
- `FfbcWebEventStore` fetches and parses calendar HTML for basic event data
- Events have: Date, Title, Town, PostalCode, Country, Province, Challenge, Notes
- Events do NOT have an ID property (needed for popup API calls)
- Caching uses `IMemoryCache` with configurable TTL (default 120 min)
- Named HTTP client "ffbc" already configured

**Constraints:**
- Must parse event ID from calendar HTML (`id` attribute on `calendar-row-informations` div)
- Popup API requires POST request with event ID and `ajax=1`
- Cache duration should be ~2 hours per user request

## Goals / Non-Goals

**Goals:**
- Extract event IDs from calendar HTML during parsing
- Create a service to fetch enriched event details from popup API
- Cache enriched details for 2 hours to minimize API calls
- Display additional fields on Event Detail page when available

**Non-Goals:**
- Pre-fetching details for all events (only on-demand)
- Modifying calendar list fetching logic beyond adding ID extraction
- Persisting enriched data permanently

## Decisions

### Decision 1: Add EventId to Event model

**Choice:** Add a nullable `EventId` property (string) to the `Event` model.

**Rationale:**
- The ID comes from external API, format unknown (could be numeric or string)
- Nullable allows backward compatibility for events without IDs
- String is safest for external identifiers

**Alternatives considered:**
- Separate EventDetails model with ID: More complex, unnecessary indirection
- Integer ID: Assumes numeric format, less flexible

### Decision 2: Create dedicated enrichment service

**Choice:** Create `IEventDetailsService` with `GetEventDetailsAsync(string eventId)` method.

**Rationale:**
- Separation of concerns: calendar listing vs. detail fetching
- Allows independent caching strategy
- Easier to test and mock

**Alternatives considered:**
- Extend `FfbcWebEventStore`: Violates single responsibility, harder to maintain
- Fetch details inline in page model: No reusability, mixes concerns

### Decision 3: Cache key strategy

**Choice:** Use cache key format `event-details::{eventId}` with 2-hour absolute expiration.

**Rationale:**
- Simple key based on unique event ID
- 2 hours matches user's requested cache duration
- Consistent with existing caching patterns in the codebase

### Decision 4: Create EventDetails model with Activity sub-model for enriched data

**Choice:** Create a separate `EventDetails` model with an `Activity` sub-model to hold structured data from the popup API.

**Rationale:**
- Keeps `Event` model focused on calendar data
- The popup HTML contains a structured `calendar-details` div with well-defined CSS classes:
  - `calendar-details-subscription` → registration URL
  - `calendar-details-place` → start location, address, Google Maps link
  - `calendar-details-club` → organizing club
  - `calendar-details-activities` → multiple `calendar-activities-row` divs, each containing:
    - `calendar-activities-row-type` → activity type (VTT, GRAVEL, MARCHE)
    - `calendar-activities-row-km` → distance
    - `calendar-activities-row-denivele` → elevation gain (D+)
    - `calendar-activities-row-dificulty` → difficulty rating
    - `calendar-activities-row-date` → departure time window
    - `calendar-activities-row-ravito` → refreshment stop count
    - `calendar-activities-row-price-ffbc` → pricing (FFBC / non-FFBC members)
  - `calendar-details-note` → organizer remarks
  - `calendar-details-website` → external website link
- Each activity row maps to an `Activity` record, displayed as a table on the detail page
- Nullable fields allow graceful handling when some data is missing (e.g. walks may omit pricing)

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| Event ID not present in calendar HTML | Extract `id` attribute from `calendar-row-informations` child div; set EventId to null if absent; detail page works without enrichment |
| Popup API format changes | Parse defensively with null checks; log parsing failures; graceful degradation |
| Popup API rate limiting | 2-hour cache reduces requests; single event fetched per detail page view |
| Network failure on detail fetch | Return null from service; detail page shows basic info only |
