## Context

The FFBC Calendar is a .NET 10 Razor Pages app deployed on Azure App Service (F1 Free). It fetches mountain biking events from the FFBC website (velo-liberte.be) via HTML scraping, geocodes them via Nominatim, and renders five views: index, list, calendar, map (Leaflet), and event detail. There is no database — all data is fetched on demand and cached in-memory (2h events, 24h geocoding). The frontend uses Bootstrap 5, custom CSS (~680 lines design system), jQuery, and Leaflet 1.9.4.

The current architecture couples server-side rendering with data fetching, which is overbuilt for a read-only site that consumes external APIs. Azure Static Web Apps offers free global CDN hosting for the frontend with integrated Azure Functions for the API layer.

## Goals / Non-Goals

**Goals:**
- Migrate to Azure Static Web Apps with SvelteKit frontend + .NET Azure Functions backend
- Maintain complete feature parity (calendar, list, map, event detail, RSS)
- Preserve the visual design and UX as closely as possible
- Reduce hosting complexity and cost
- Clean separation of frontend (static) and backend (API)

**Non-Goals:**
- Adding new features, authentication, or user accounts
- Changing external data sources (FFBC, Nominatim)
- Redesigning the UI or branding
- Introducing a database or persistent storage
- Server-side rendering in SvelteKit (pure static/SPA mode)

## Decisions

### 1. Frontend Framework: SvelteKit with `@sveltejs/adapter-static`

**Choice:** SvelteKit  
**Alternatives considered:**
- **React/Next.js**: Larger bundle size (~40-80KB min+gzip for React alone), more boilerplate, overkill for 5 pages
- **Vue/Nuxt**: Middle ground but still heavier than Svelte, less compelling DX advantage
- **Blazor WASM**: Stays in .NET but 2-5MB initial download, slow startup, poor SEO
- **Vanilla JS/HTML**: Lightest but no component model, routing, or dev tooling; harder to maintain
- **Astro**: Great for static content but less natural for SPA-like interactivity (map, calendar nav)

**Rationale:** SvelteKit produces the smallest bundles of any component framework (compiler-based, no runtime overhead). It has file-based routing matching the current page structure, built-in static adapter for SWA, reactive declarations without boilerplate, and excellent DX. The 5-page app with interactive map and calendar is a perfect fit.

### 2. Backend: .NET Isolated Azure Functions (in-process v4)

**Choice:** .NET Isolated Worker  
**Rationale:** Reuses the existing C# services (FfbcWebEventStore, FfbcEventDetailsService, NominatimGeocodingService) with minimal changes. The HTML parsing logic with HtmlAgilityPack and regex-based location extraction is battle-tested and covered by existing tests.

**API Endpoints:**
| Endpoint | Method | Maps to |
|---|---|---|
| `/api/events` | GET | `IEventStore.GetAll()` |
| `/api/events/{eventId}/details` | GET | `IEventDetailsService.GetEventDetailsAsync()` |
| `/api/geocode/{postalCode}` | GET | `IGeocodingService.GeocodeAsync()` |
| `/api/feed.xml` | GET | RSS feed generation |

### 3. Project Structure

```
ffbc-calendar/
├── src/
│   ├── WebApp/              → SvelteKit frontend (replaces Razor Pages)
│   │   ├── src/
│   │   │   ├── routes/      → File-based routing (/, /list, /calendar, /map, /event)
│   │   │   ├── lib/         → Shared components, API client, types
│   │   │   └── app.html     → Root template
│   │   ├── static/          → favicon, manifest
│   │   ├── svelte.config.js
│   │   ├── package.json
│   │   └── vite.config.ts
│   └── Api/                 → .NET Azure Functions project
│       ├── Functions/       → HTTP trigger functions
│       ├── Services/        → Migrated from current WebApp/Services
│       ├── Models/          → Migrated from current WebApp/Models
│       ├── Options/         → Migrated from current WebApp/Options
│       └── Api.csproj
├── tests/
│   ├── Api.Tests/           → .NET function tests (migrated from WebApp.Tests)
│   └── WebApp.Tests/        → Svelte component/integration tests (Vitest)
└── staticwebapp.config.json → SWA routing config
```

### 4. CSS Strategy: Preserve existing design system

**Choice:** Port the existing `site.css` design system (CSS custom properties, Bootstrap overrides) directly to SvelteKit. Use Bootstrap 5 via npm package instead of local wwwroot copy.

**Rationale:** The 680-line CSS file is a well-structured design system with tokens, components, and responsive breakpoints. Porting it preserves the exact visual identity with no redesign needed. Svelte's scoped CSS can handle component-specific styles.

### 5. State Management: Svelte stores for client-side caching

**Choice:** Use Svelte writable stores to cache API responses client-side (events list, geocoding results).  
**Rationale:** Simple, built-in, and sufficient. The app has no complex state — just cached API responses. No need for external state management libraries.

### 6. Map Integration: Leaflet via `svelte-leaflet` or direct binding

**Choice:** Use Leaflet directly with Svelte component wrappers (not a library like `svelte-leaflet`).  
**Rationale:** The existing map code is ~50 lines of JS. Wrapping it in a Svelte component with `onMount` is simpler and more maintainable than adding a third-party Svelte-Leaflet binding.

### 7. SWA Configuration

The `staticwebapp.config.json` will handle:
- Route fallback to `index.html` for SPA client-side routing
- API route proxying to the managed Functions backend
- RSS feed route (`/feed.xml` → `/api/feed.xml`)
- Cache headers for static assets

## Risks / Trade-offs

| Risk | Mitigation |
|---|---|
| **Client-side rendering loses SEO** | This is a niche Belgian mountain biking calendar — SEO is not critical. RSS feed provides syndication. SvelteKit can add `<head>` meta tags at build time. |
| **Cold start latency on Azure Functions** | The Consumption plan has cold starts (~1-3s). Mitigate with SWA managed functions which stay warm longer. Events are cached, so subsequent requests are fast. |
| **Leaflet SSR incompatibility** | Leaflet requires `window`/`document`. Use dynamic import with `onMount` in Svelte — only load Leaflet client-side. |
| **CORS for external API calls** | External FFBC API is called server-side from Functions, not from the browser. No CORS issues. |
| **Test migration effort** | Existing xUnit tests test C# services which mostly carry over. Integration tests need rewriting for Function endpoints. Frontend tests are new (Vitest + testing-library). |
| **In-memory caching in serverless** | Azure Functions on Consumption plan may lose cache between invocations. Use `IMemoryCache` — cache misses just trigger a fresh fetch. Acceptable given 2h TTL. |

## Migration Plan

1. **Phase 1 — API Layer**: Create the `src/Api` Functions project, migrate services/models/options, expose HTTP endpoints, migrate and adapt existing tests
2. **Phase 2 — Frontend**: Create the SvelteKit project in `src/WebApp`, build all 5 pages, port CSS design system, wire up API client
3. **Phase 3 — Infrastructure**: Create `staticwebapp.config.json`, rewrite GitHub Actions workflow for SWA deployment
4. **Phase 4 — Cleanup**: Remove old Razor Pages code, update README, verify parity

**Rollback:** The old App Service deployment remains live until the SWA is verified. The GitHub Actions workflow change is the final switch — reverting it restores the original deployment.

## Open Questions

- Should we keep the `FFBC.slnx` solution file or switch to a more standard `FFBC.sln`? (Will keep `.slnx` for consistency)
- Should the Functions project use in-process or isolated worker model? (Decision: Isolated — it's the recommended model going forward)
