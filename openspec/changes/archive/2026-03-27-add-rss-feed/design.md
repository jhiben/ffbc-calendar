## Context

The FFBC Calendar is a .NET 10 Razor Pages app serving mountain bike events. Events are fetched from an external FFBC API and cached in-memory via `IEventStore.GetAll()`. The `Event` model has: EventId, Date, Title, Notes, Town, PostalCode, Country, Province, Challenge.

There is no current feed endpoint. The app uses Razor Pages routing exclusively — no minimal API endpoints yet. Adding a `/feed.xml` route requires a `MapGet` in `Program.cs`.

## Goals / Non-Goals

**Goals:**
- Serve a valid RSS 2.0 XML feed at `/feed.xml`
- Map each upcoming `Event` to an RSS `<item>` with title, description (location + notes), publication date, and link to detail page
- Support RSS auto-discovery via `<link>` tag in `<head>`
- Provide a visible feed link for manual subscription

**Non-Goals:**
- Atom format
- Filtered/category feeds
- Feed caching layer (events are already cached by `IEventStore`)
- Feed authentication

## Decisions

### Decision 1: `System.ServiceModel.Syndication` for RSS generation

**Choice:** Use the `System.ServiceModel.Syndication` NuGet package to build the `SyndicationFeed` and serialize to RSS 2.0 XML.

**Rationale:** This is the official .NET library for syndication feeds, well-maintained, supports RSS 2.0 out of the box, and avoids manual XML construction.

**Alternatives considered:**
- Manual XML with `XDocument` — error-prone for spec compliance, no benefit
- Third-party libraries (`SimpleFeed`, etc.) — unnecessary dependency when the official package exists

### Decision 2: Minimal API endpoint in Program.cs

**Choice:** Add `app.MapGet("/feed.xml", handler)` in `Program.cs` rather than creating a Razor Page.

**Rationale:** The feed returns XML, not HTML. A minimal API endpoint is cleaner for non-page responses. The handler injects `IEventStore`, builds the feed, and returns `Results.Content(xml, "application/rss+xml")`.

**Alternatives considered:**
- Razor Page returning XML — awkward, Razor is designed for HTML
- Dedicated controller — overkill for one endpoint, app doesn't use MVC controllers

### Decision 3: Inline handler, no separate service class

**Choice:** Implement the feed generation logic directly in the `MapGet` lambda (or a small static method in Program.cs area), not as a separate `RssFeedService`.

**Rationale:** The logic is ~20 lines: get events, map to `SyndicationItem`, serialize. A separate service class adds ceremony without value. If feed logic grows in the future, it can be extracted.

## Risks / Trade-offs

- **Event link generation** → The feed handler needs to construct absolute URLs for event detail pages. Use `HttpContext.Request` to derive the base URL at runtime.
- **Empty feed** → If no events are loaded yet, the feed returns a valid RSS document with zero items. This is valid RSS.
- **Content type** → Some readers expect `application/rss+xml`, others accept `text/xml`. Using `application/rss+xml` is the standard.
