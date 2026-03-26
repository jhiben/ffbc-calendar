## Why

Users currently see a map centered on Belgium by default, requiring them to manually pan and zoom to find events near their location. By centering the map on the user's actual location with a 50km radius view, users can immediately see nearby events without manual navigation, improving discovery and engagement.

## What Changes

- Add browser-based geolocation to detect the user's current position
- Center the map on the user's location when permission is granted
- Set a default zoom level that approximates a 50km radius view
- Fall back to the current Belgium-centered view if geolocation is denied or unavailable
- Add a "center on my location" button for re-centering after panning

## Capabilities

### New Capabilities

- `user-geolocation`: Request and handle browser geolocation to obtain the user's current position, with appropriate permission handling and fallback behavior

### Modified Capabilities

- `map-view`: Change default map centering behavior to use user location when available instead of always centering on Belgium

## Impact

- `src/WebApp/Pages/Map.cshtml`: Add JavaScript for Geolocation API calls and map centering logic
- User experience: First-time visitors will see a browser permission prompt for location access
- Privacy: Location data is used client-side only for map centering, not stored or transmitted

## Non-goals

- Storing or persisting user location data
- Server-side geolocation or IP-based location detection
- Radius filtering of events (all events still shown, just different initial view)
- Location sharing with other users or external services
