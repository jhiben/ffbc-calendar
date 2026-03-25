## Why

Events are currently shown as compact rows in the list, calendar, and map views with minimal information (title, date, notes). Users cannot see full event details — such as the challenge type, location breakdown, or a dedicated view for a single event. Adding a detail page gives each event a permalink and a richer presentation, especially useful for MTB ride information.

## What Changes

- Add a new **Event Detail** Razor page (`/EventDetail`) that displays all available fields for a single event.
- Enrich the `Event` model with additional fields parsed from the FFBC source (challenge type, province, detail URL if available).
- Update the `FfbcWebEventStore` parser to capture extra data from the HTML calendar rows.
- Add links to the detail page from the **List View**, **Calendar**, and **Map** pages wherever events are displayed.

## Non-goals

- No external API calls beyond what the FFBC calendar already provides (no scraping individual event pages on the FFBC site).
- No database or persistence changes — events remain in-memory.
- No authentication or authorization for the detail page.

## Capabilities

### New Capabilities
- `event-detail-page`: A dedicated page that displays full details of a single event, identified by a combination of date and title.
- `enriched-event-model`: Extend the Event model and parser to capture additional fields from the FFBC calendar HTML (challenge type, province).

### Modified Capabilities
- `list-view`: Each event row links to the event detail page.
- `calendar-view`: Each event badge links to the event detail page.
- `map-view`: Map marker popups and the unmapped-events table link to the event detail page.

## Impact

- **Models**: `Event.cs` gains new optional properties (`Challenge`, `Province`).
- **Services**: `FfbcWebEventStore.TryParseEvents` stores challenge and province separately instead of only in `Notes`.
- **Pages**: New `EventDetail.cshtml` / `EventDetail.cshtml.cs`. Modifications to `ListEvents.cshtml`, `Calendar.cshtml`, `Map.cshtml` to add links.
- **Tests**: New tests for the detail page model; updated parsing tests for new fields; tests verifying links render on existing pages.
