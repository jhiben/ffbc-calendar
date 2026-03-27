## 1. Dependencies

- [x] 1.1 Add `System.ServiceModel.Syndication` NuGet package to `WebApp.csproj`

## 2. Feed Endpoint

- [x] 2.1 Add `MapGet("/feed.xml", ...)` endpoint in `Program.cs` that injects `IEventStore` and `HttpContext`
- [x] 2.2 Build `SyndicationFeed` with channel title ("FFBC Calendar Events"), link (site base URL), and description
- [x] 2.3 Map each event from `IEventStore.GetAll()` to a `SyndicationItem` with title, link (absolute URL to `/EventDetail`), description (town + country + notes), and pubDate
- [x] 2.4 Serialize the feed to RSS 2.0 XML using `Rss20FeedFormatter` and return with content type `application/rss+xml`

## 3. Auto-Discovery & Visibility

- [x] 3.1 Add `<link rel="alternate" type="application/rss+xml" title="FFBC Calendar Events" href="/feed.xml" />` to `<head>` in `_Layout.cshtml`
- [x] 3.2 Add a visible RSS link (🔔 or RSS label) in the footer of `_Layout.cshtml`

## 4. Tests

- [x] 4.1 Write a test that verifies `/feed.xml` returns 200 with content type `application/rss+xml`
- [x] 4.2 Write a test that verifies the feed contains `<rss version="2.0">` and `<channel>` elements
- [x] 4.3 Write a test that verifies feed items map correctly from events (title, link, pubDate)
- [x] 4.4 Write a test that verifies an empty event store returns a valid RSS feed with no items

## 5. Build & Verify

- [x] 5.1 Run `dotnet build` and confirm zero errors
- [x] 5.2 Run `dotnet test` and confirm all tests pass (existing + new)
