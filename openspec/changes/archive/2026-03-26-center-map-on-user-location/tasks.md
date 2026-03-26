## 1. Geolocation Module

- [x] 1.1 Create a JavaScript function to request user geolocation with 5-second timeout
- [x] 1.2 Add error handling for permission denied, unavailable, and timeout scenarios
- [x] 1.3 Return a Promise that resolves with coordinates or null on failure

## 2. Map Initialization Update

- [x] 2.1 Modify map initialization to call geolocation before setting view
- [x] 2.2 If geolocation succeeds, center map at user coordinates with zoom level 10
- [x] 2.3 If geolocation fails, fall back to existing Belgium-centered behavior with marker auto-fit
- [x] 2.4 Ensure marker display still works correctly after geolocation-based centering

## 3. Re-center Control

- [x] 3.1 Create a Leaflet custom control for the location button in bottom-right corner
- [x] 3.2 Add click handler to request geolocation and re-center map on success
- [x] 3.3 Style the button to match existing map UI (use location/crosshair icon)

## 4. Testing & Verification

- [x] 4.1 Test with geolocation permission granted (map centers on user location)
- [x] 4.2 Test with geolocation permission denied (graceful fallback to Belgium view)
- [x] 4.3 Test re-center button functionality after panning the map
- [x] 4.4 Verify all existing map marker and popup functionality still works
