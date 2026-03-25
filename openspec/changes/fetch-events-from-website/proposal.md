## Why

The app currently uses a hard-coded in-memory list of events. The official FFBC website (velo-liberte.be) maintains the authoritative schedule of mountain biking and hiking events. Replacing the stub store with a live HTTP fetch means riders always see real, up-to-date events without any manual data entry.

## What Changes

- **BREAKING**: Replace `InMemoryEventStore` (hard-coded seed data) with a new `FfbcWebEventStore` that fetches events from the FFBC website via HTTP POST
- Add in-memory caching of fetched results with a configurable TTL (default 2 hours) to avoid hammering the remote API on every page load
- Keep the existing `IEventStore` interface unchanged so all consumers (list view, calendar view) continue to work without modification

## Capabilities

### New Capabilities

- `remote-event-store`: Fetch mountain biking events from the FFBC website via HTTP POST, parse the HTML response, and return a list of `Event` objects cached for a configurable duration

### Modified Capabilities

<!-- No existing spec-level behavior changes — IEventStore contract is unchanged -->

## Non-goals

- Storing fetched events in a database or persistent file cache
- Supporting other provinces or challenge types beyond what is passed as configuration
- Displaying a loading indicator or partial results during fetch
- Authentication or session management with the remote API

## Impact

- `InMemoryEventStore` replaced by `FfbcWebEventStore` as the `IEventStore` singleton registration in `Program.cs`
- New dependency: `HtmlAgilityPack` (or `AngleSharp`) for HTML parsing
- `HttpClient` registered via `IHttpClientFactory`
- `IMemoryCache` registered for result caching
- No changes to `IEventStore`, `Event`, or any Razor Pages
