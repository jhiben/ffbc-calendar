## Why

The app currently runs on Azure App Service as a server-rendered Razor Pages application. This is more infrastructure than needed for what is essentially a read-only, content-driven site backed by external APIs. Migrating to **Azure Static Web Apps** with a **SvelteKit** frontend and **.NET Azure Functions** backend will reduce hosting costs (SWA free tier is more generous), improve performance through CDN-distributed static assets, and simplify the architecture by cleanly separating the UI from the API layer.

## What Changes

- **BREAKING**: Replace the ASP.NET Razor Pages frontend with a SvelteKit static SPA using the `@sveltejs/adapter-static`
- **BREAKING**: Move server-side logic (event fetching, HTML parsing, geocoding, event details, RSS feed) into a .NET Isolated Azure Functions project under `/api`
- **BREAKING**: Replace the GitHub Actions deploy workflow to target Azure Static Web Apps instead of App Service
- Maintain full feature parity: calendar view, event list, interactive map, event detail pages, RSS feed
- Keep the existing visual design (Bootstrap + custom CSS design system, Leaflet maps)
- Expose REST API endpoints from Azure Functions: `/api/events`, `/api/events/{id}/details`, `/api/geocode/{postalCode}`, `/api/feed.xml`
- Reuse existing C# services (FfbcWebEventStore, FfbcEventDetailsService, NominatimGeocodingService) in the Functions project
- Rewrite tests to cover the new API layer and add frontend component tests

## Non-goals

- Changing the external data sources (velo-liberte.be, Nominatim)
- Adding new features beyond current parity (auth, user accounts, favorites)
- Changing the visual design or branding
- Migrating to a database — the in-memory caching approach remains valid in Azure Functions

## Capabilities

### New Capabilities
- `sveltekit-frontend`: SvelteKit SPA with static adapter — pages for index, calendar, list, map, event detail
- `functions-api`: .NET Isolated Azure Functions exposing REST endpoints for events, event details, geocoding, and RSS feed
- `swa-deployment`: Azure Static Web Apps deployment via GitHub Actions with SWA CLI

### Modified Capabilities
- `azure-deployment`: Deployment target changes from App Service to Static Web Apps
- `rss-feed`: RSS generation moves from inline Razor endpoint to Azure Function endpoint
- `remote-event-store`: Event fetching moves from singleton service to Function-scoped with caching
- `map-view`: Map rendering moves from server-prepared marker JSON to client-side API fetch
- `calendar-view`: Calendar grid logic moves from server-side C# to client-side Svelte
- `event-detail-page`: Detail enrichment moves from server-side page model to client-side API call
- `list-view`: Event listing moves from server-side to client-side with API fetch

## Impact

- **Code**: Entire `src/WebApp` project restructured — Razor Pages removed, SvelteKit project added, Functions project added
- **Dependencies**: New npm dependencies (SvelteKit, adapter-static, Bootstrap). NuGet dependencies shift to Functions project
- **APIs**: New REST API surface exposed via Azure Functions (currently no public API)
- **CI/CD**: GitHub Actions workflow rewritten for SWA build/deploy
- **Infrastructure**: Azure App Service replaced by Azure Static Web Apps resource
- **Tests**: Existing integration tests need rewriting for the new Function endpoints; new frontend tests added
