## Why

Users cannot subscribe to upcoming FFBC mountain bike events in their preferred feed reader. An RSS feed lets riders stay up-to-date without visiting the site, and enables integration with calendaring tools and automation services (IFTTT, Zapier, etc.).

## What Changes

- Add an RSS 2.0 feed endpoint at `/feed.xml` serving upcoming events
- Each event maps to an RSS `<item>` with title, date, location, and a link to the event detail page
- Add a `<link rel="alternate" type="application/rss+xml">` tag in the layout `<head>` for feed auto-discovery
- Add a feed icon/link in the navbar or footer for manual discovery
- Add `System.ServiceModel.Syndication` NuGet package for RSS generation

## Capabilities

### New Capabilities

- `rss-feed`: RSS 2.0 feed endpoint serving upcoming events with auto-discovery

### Modified Capabilities

*(No existing spec-level behavior changes)*

## Impact

- `WebApp.csproj` — new package reference: `System.ServiceModel.Syndication`
- `Program.cs` — new minimal API endpoint: `MapGet("/feed.xml", ...)`
- `Pages/Shared/_Layout.cshtml` — RSS auto-discovery `<link>` in `<head>`, optional feed link in nav/footer
- New file: `Services/RssFeedService.cs` or inline handler

## Non-goals

- Atom feed support (RSS 2.0 only for now)
- Per-category or filtered feeds (single feed of all upcoming events)
- Feed pagination or archival of past events
- Authentication or rate limiting on the feed endpoint
