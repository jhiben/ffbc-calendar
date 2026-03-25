## Context

The app currently registers `InMemoryEventStore` as a singleton with four hard-coded seed events. The FFBC website exposes event data via a POST endpoint that returns a fragment of HTML. This change replaces the stub with a real HTTP-backed store. The `IEventStore` interface and all consumers stay unchanged. The primary design concerns are: how to send the right request, how to parse the HTML response, and how to avoid excessive outbound requests via caching.

## Goals / Non-Goals

**Goals:**
- Implement `FfbcWebEventStore : IEventStore` that POSTs to the FFBC API and parses the HTML response into `Event` objects
- Cache the result in `IMemoryCache` with a configurable TTL (default 2 hours) keyed by request parameters
- Register `FfbcWebEventStore` in place of `InMemoryEventStore` in `Program.cs`
- Keep request parameters (province, challenge, year, date) configurable via `appsettings.json`

**Non-Goals:**
- Persistent caching (file, Redis, database)
- Supporting dynamic province/challenge switching from the UI
- Error UI — on fetch failure fall back to an empty list (fail-safe, non-crashing)

## Decisions

### `HttpClient` via `IHttpClientFactory` with a named client

**Decision**: Register a named `HttpClient` (`"ffbc"`) in `Program.cs` with the base address and any default headers. Inject `IHttpClientFactory` into `FfbcWebEventStore`.

**Rationale**: Avoids socket exhaustion from newing up `HttpClient`; named client centralises configuration and is easy to mock in tests.

**Alternatives considered**:
- _Typed `HttpClient`_: Works but couples DI registration more tightly; named client is simpler for a single endpoint

### `HtmlAgilityPack` for HTML parsing

**Decision**: Use `HtmlAgilityPack` to parse the response HTML.

**Rationale**: Mature, widely used, tolerant of malformed HTML, available on NuGet. The response is a server-rendered HTML fragment — XPath/CSS selectors are sufficient.

**Alternatives considered**:
- _AngleSharp_: More modern API but async-only and heavier; unnecessary for a small fragment
- _Regex_: Brittle against markup changes

### `IMemoryCache` with a sliding-expiry cache entry

**Decision**: Cache the parsed `IReadOnlyList<Event>` in `IMemoryCache` using an absolute expiry of the configured TTL.

**Rationale**: `IMemoryCache` is already available in ASP.NET Core without additional packages. Absolute expiry ensures data is periodically refreshed regardless of traffic patterns.

### Request parameters via `FfbcEventStoreOptions` (Options pattern)

**Decision**: Bind `appsettings.json` section `"FfbcEventStore"` to a `FfbcEventStoreOptions` POCO (PostUrl, Province, Challenge, Year, CacheDurationMinutes).

**Rationale**: Keeps secrets and environment-specific values out of code; easy to override per environment.

## Risks / Trade-offs

| Risk | Mitigation |
|---|---|
| FFBC website HTML structure changes → parse breaks | Parse defensively; return empty list on error; log warning |
| Remote call fails (network error, 5xx) → page shows no events | Catch `HttpRequestException`; return empty list; do not cache failures |
| Remote API changes request format → POST returns garbage | Treat unexpected response body as a parse failure (empty list) |
| Cache prevents seeing freshly added events | TTL is configurable; default 120 min is acceptable for a planning app |

## Open Questions

- Should failed requests be cached for a short negative TTL to avoid thundering-herd on outages? (Not in scope for this change — treat as future enhancement)
