## Context

The map page currently initializes with a hardcoded center on Belgium (`[50.5, 4.5]`) at zoom level 8. When events are present, it auto-fits to show all markers. This approach works but doesn't personalize the experience for users who want to see events near their current location.

The change introduces browser-based geolocation via the Geolocation API to center the map on the user's position. Since this is a client-side JavaScript enhancement to an existing Razor page, no backend changes are needed.

**Current state:**
- Map is rendered in `Map.cshtml` using Leaflet.js
- Default center: Belgium `[50.5, 4.5]`, zoom: 8
- If markers exist, map fits to marker bounds

**Constraints:**
- Must work without backend changes (client-side only)
- Must handle geolocation permission denial gracefully
- Must not break existing marker display functionality

## Goals / Non-Goals

**Goals:**
- Center map on user's location when geolocation is available and permitted
- Provide a zoom level approximating 50km radius visibility
- Graceful fallback to current Belgium-centered behavior
- Allow users to re-center on their location after panning

**Non-Goals:**
- Storing or persisting user location
- Server-side IP-based geolocation
- Filtering events by distance from user
- Tracking user movement in real-time

## Decisions

### Decision 1: Use Browser Geolocation API

**Choice:** Use the native `navigator.geolocation.getCurrentPosition()` API.

**Rationale:** 
- Built into all modern browsers, no external dependencies
- Provides accurate GPS coordinates on mobile devices
- Users explicitly grant permission, maintaining trust

**Alternatives considered:**
- IP-based geolocation: Less accurate, requires server-side implementation
- Manual location input: Extra friction for users

### Decision 2: Zoom level for 50km radius

**Choice:** Use zoom level 10 for the 50km radius approximation.

**Rationale:**
- At latitude ~50° (Belgium), zoom 10 shows roughly 50-60km horizontally
- Leaflet zoom formula: `resolution = 156543.03 * cos(lat) / 2^zoom` meters/pixel
- Zoom 10 provides good balance between seeing nearby events and maintaining context

**Alternatives considered:**
- Zoom 9: ~100km view, too zoomed out for "nearby" feel
- Zoom 11: ~25km view, might miss relevant events
- Dynamic calculation: Unnecessary complexity for this use case

### Decision 3: Geolocation timing

**Choice:** Request geolocation immediately on page load, before fitting to markers.

**Rationale:**
- Provides fastest path to user-centered view
- If geolocation succeeds, use it; otherwise fall back to current behavior
- Avoids jarring re-centering after markers are already displayed

### Decision 4: Re-center button placement

**Choice:** Add a floating button in the bottom-right corner of the map.

**Rationale:**
- Common UX pattern (Google Maps, OpenStreetMap apps)
- Doesn't interfere with marker popups
- Uses Leaflet's custom control positioning

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| User denies geolocation permission | Graceful fallback to current Belgium-centered behavior; no error shown to user |
| Geolocation takes too long | Set 5-second timeout; fall back to default view if exceeded |
| User is far from Belgium (no events visible) | Map still shows all event markers; user can zoom out or click "fit all events" if added later |
| HTTPS requirement for geolocation | Already required for production; development can use localhost |
