## Context

The FFBC WebApp currently offers three views of MTB events — a chronological list, a monthly calendar, and a map. Each view shows minimal event information (title, date, notes). The `Event` model has fields for `Date`, `Title`, `Notes`, `Town`, `PostalCode`, and `Country`. The `FfbcWebEventStore` parser already extracts challenge type and province from the HTML but collapses them into the catch-all `Notes` string. There is no way to view a single event's full details or link to it directly.

## Goals / Non-Goals

**Goals:**
- Provide a dedicated event detail page accessible via a unique URL per event.
- Enrich the `Event` model with structured fields for challenge type and province (already available in the HTML source).
- Add clickable links from the calendar, list, and map views to the detail page.

**Non-Goals:**
- Scraping the individual event pages on the FFBC website for even more information.
- Adding a database or any persistence mechanism.
- Implementing full-text search or filtering on the detail page.

## Decisions

### 1. Event identification via slug: date + title

Events have no unique ID from the FFBC source. We will identify events by combining the date (`yyyy-MM-dd`) and a URL-friendly slug derived from the title. The detail page route will be `/EventDetail?date=2026-03-25&title=rallye-des-spirous`.

**Rationale**: This avoids adding synthetic IDs to the model and leverages the natural uniqueness of date + event name within the FFBC calendar. A query-string approach is simpler than custom route constraints and aligns with existing Razor Pages patterns in the project.

**Alternative considered**: Integer index into the event list — fragile because the list order could change between requests (cache refresh).

### 2. Extend Event model with Challenge and Province fields

Add nullable `string? Challenge` and `string? Province` properties to `Event`. Update `FfbcWebEventStore.TryParseEvents` to populate them from the already-extracted `challengeText` and province portion of `placeText`. Continue computing `Notes` as the combined string for backward compatibility.

**Rationale**: The data is already parsed but discarded into a single notes string. Structured fields enable the detail page to display them in a richer layout without re-parsing.

**Alternative considered**: Parsing `Notes` on the detail page — brittle and duplicates logic.

### 3. Lookup method on IEventStore

Add `Event? GetByDateAndTitle(DateTime date, string title)` to `IEventStore`. This keeps event lookup logic in the store and avoids exposing the internal list for filtering in the page model.

**Rationale**: Encapsulates the lookup; testable via the interface. Both `InMemoryEventStore` and `FfbcWebEventStore` can implement it by filtering `GetAll()`.

### 4. Links use anchor tags with query parameters

From existing views, link to the detail page using Razor `asp-page` / `asp-route-*` tag helpers. The slug construction (lowercased, hyphenated title) is done in a shared static utility to keep it consistent.

## Risks / Trade-offs

- **Title collisions on the same date** → Unlikely in the FFBC calendar domain (distinct event names). Mitigation: return the first match; no user-facing error expected.
- **Slug encoding edge cases** → Non-ASCII characters in event titles (common in French). Mitigation: URL-encode the title parameter and do case-insensitive matching in the store.
- **Cache refresh changes available events** → A user could bookmark a detail URL for an event that disappears from the next fetch. Mitigation: show a "not found" message on the detail page instead of a 500.
