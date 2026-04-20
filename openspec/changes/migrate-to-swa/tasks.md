## 1. Azure Functions API Project Setup

- [x] 1.1 Create `src/Api` .NET Isolated Azure Functions project (`Api.csproj`) targeting .NET 10 with `Microsoft.Azure.Functions.Worker` and `Microsoft.Azure.Functions.Worker.Extensions.Http` packages
- [x] 1.2 Migrate `Models/` (Event, EventDetails, Activity) from `src/WebApp/Models` to `src/Api/Models`
- [x] 1.3 Migrate `Options/FfbcEventStoreOptions.cs` from `src/WebApp/Options` to `src/Api/Options`
- [x] 1.4 Migrate `Services/` (IEventStore, FfbcWebEventStore, IEventDetailsService, FfbcEventDetailsService, IGeocodingService, NominatimGeocodingService) from `src/WebApp/Services` to `src/Api/Services`
- [x] 1.5 Configure `Program.cs` with dependency injection: register IMemoryCache, HttpClient factories, IEventStore, IEventDetailsService, IGeocodingService as singletons, bind FfbcEventStoreOptions from configuration
- [x] 1.6 Create `local.settings.json` with FfbcEventStore configuration values matching current `appsettings.json`
- [x] 1.7 Add `Api` project to `FFBC.slnx` solution file

## 2. Azure Functions HTTP Endpoints

- [x] 2.1 Implement `GET /api/events` function: calls `IEventStore.GetAll()`, serializes events as JSON array ordered by date ascending
- [x] 2.2 Implement `GET /api/events/{eventId}/details` function: calls `IEventDetailsService.GetEventDetailsAsync()`, returns JSON or 404
- [x] 2.3 Implement `GET /api/geocode/{postalCode}` function: accepts optional `country` query param, calls `IGeocodingService.GeocodeAsync()`, returns JSON coordinates or 404
- [x] 2.4 Implement `GET /api/feed.xml` function: generates RSS 2.0 feed from events using `SyndicationFeed`, returns `application/rss+xml` response with items linking to `/event?date=...&title=...`

## 3. API Tests

- [x] 3.1 Create `tests/Api.Tests` project with xUnit, reference `src/Api`, add test infrastructure (stub HTTP handlers, test helpers)
- [x] 3.2 Migrate and adapt existing service unit tests (FfbcWebEventStoreTests, FfbcEventDetailsServiceTests, NominatimGeocodingServiceTests) to `Api.Tests`
- [x] 3.3 Write integration tests for `GET /api/events` endpoint (success, empty, ordering)
- [x] 3.4 Write integration tests for `GET /api/events/{eventId}/details` endpoint (success, 404, activity content)
- [x] 3.5 Write integration tests for `GET /api/geocode/{postalCode}` endpoint (success, 404, country param)
- [x] 3.6 Write integration tests for `GET /api/feed.xml` endpoint (valid RSS, items, empty feed)
- [x] 3.7 Add `Api.Tests` project to `FFBC.slnx` solution file

## 4. SvelteKit Project Setup

- [x] 4.1 Scaffold SvelteKit project in `src/WebApp` (after backing up/removing current Razor Pages code): `npm create svelte@latest` with TypeScript, `@sveltejs/adapter-static`
- [x] 4.2 Install dependencies: `bootstrap` (npm), `leaflet`, `@types/leaflet`
- [x] 4.3 Port `site.css` design system to `src/WebApp/src/app.css` — all CSS custom properties, Bootstrap overrides, component styles
- [x] 4.4 Create `src/WebApp/src/app.html` root template with RSS auto-discovery `<link>` tag
- [x] 4.5 Configure `svelte.config.js` with static adapter, SPA fallback, and path configuration
- [x] 4.6 Configure `vite.config.ts` with proxy to local Functions (`/api` → `http://localhost:7071`) for development

## 5. SvelteKit Shared Components & API Client

- [x] 5.1 Create TypeScript type definitions in `src/WebApp/src/lib/types.ts` matching API response shapes (Event, EventDetails, Activity, GeoCoord)
- [x] 5.2 Create API client module `src/WebApp/src/lib/api.ts` with functions: `fetchEvents()`, `fetchEventDetails(eventId)`, `geocode(postalCode, country?)`, all with error handling
- [x] 5.3 Create Svelte stores `src/WebApp/src/lib/stores.ts` for events cache, geocoding cache
- [x] 5.4 Create layout component `src/WebApp/src/routes/+layout.svelte` with navbar (brand, nav links with active state), content slot, footer (copyright, privacy, RSS link)

## 6. SvelteKit Pages

- [x] 6.1 Create index page `src/WebApp/src/routes/+page.svelte` — hero section with gradient background, 3 navigation cards (List, Calendar, Map)
- [x] 6.2 Create list page `src/WebApp/src/routes/list/+page.svelte` — fetch events from API, render responsive table with date/title/notes, title links to event detail, loading state, empty state
- [x] 6.3 Create calendar page `src/WebApp/src/routes/calendar/+page.svelte` — fetch events, compute month grid client-side (Mon-Sun weeks), render day cells with event chips, prev/next month navigation, today highlight, support `?year=&month=` params
- [x] 6.4 Create map page `src/WebApp/src/routes/map/+page.svelte` — fetch events, geocode by unique postal codes via API, render Leaflet map with markers/popups, user geolocation with timeout, "Locate Me" button, unmapped events table below map
- [x] 6.5 Create event detail page `src/WebApp/src/routes/event/+page.svelte` — read `?date=&title=` params, fetch event + enriched details from API, render general info card, additional details card, activity cards with emoji metrics and color-coded difficulty badges, loading state, not-found state

## 7. Static Web Apps Configuration

- [x] 7.1 Create `staticwebapp.config.json` at repo root: navigation fallback to `index.html`, `/feed.xml` → `/api/feed.xml` rewrite, cache headers for static assets
- [x] 7.2 Rewrite `.github/workflows/deploy.yml` for SWA: install Node.js + npm build frontend, dotnet build + publish API, run all tests, deploy with `Azure/static-web-apps-deploy` using `AZURE_STATIC_WEB_APPS_API_TOKEN` secret

## 8. Frontend Tests

- [x] 8.1 Set up Vitest + `@testing-library/svelte` in the SvelteKit project
- [x] 8.2 Write component tests for layout (navbar links, active state, footer RSS link)
- [x] 8.3 Write component tests for list page (renders events, empty state, loading state)
- [x] 8.4 Write component tests for calendar page (grid layout, month navigation, event badges, today highlight)
- [x] 8.5 Write component tests for event detail page (renders fields, enriched details, not-found state)

## 9. Cleanup & Documentation

- [x] 9.1 Remove old Razor Pages files from `src/WebApp` (*.cshtml, *.cshtml.cs, WebApp.csproj, Properties/, wwwroot/lib/)
- [x] 9.2 Remove old `tests/WebApp.Tests` project (replaced by `tests/Api.Tests` + frontend Vitest)
- [x] 9.3 Update `FFBC.slnx` to remove old WebApp and WebApp.Tests projects
- [x] 9.4 Update `README.md` with new getting started instructions (npm install, npm run dev, dotnet run for API, SWA CLI for full local dev), deployment instructions (SWA setup, GitHub secret), and architecture overview
- [x] 9.5 Verify full local dev workflow: SvelteKit dev server + Functions host running together, all pages functional, RSS feed accessible
