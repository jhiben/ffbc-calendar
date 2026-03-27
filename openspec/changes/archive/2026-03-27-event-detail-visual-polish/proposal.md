## Why

The event detail page's activities table and detail sections are plain text in a flat table. For a mountain biking audience, key metrics like distance, elevation gain, difficulty, and price deserve visual indicators — emojis, color-coded badges, and icons — so riders can scan and compare routes at a glance.

## What Changes

- Replace the plain activities table with visually rich activity cards, each showing distance, D+, difficulty, time, ravito, and price with dedicated emoji labels and color-coded badges
- Add color-coded difficulty indicators (green/yellow/orange/red) for difficulty values
- Add visual formatting for distance and elevation (emoji + styled values)
- Add price display with cost indicator styling (free vs paid)
- Add ravito (refreshment) visual indicator
- Enhance the detail section labels (Date, Town, Club, etc.) with emoji prefixes for scannability
- Add CSS for the new activity card layout and visual indicator styles

## Capabilities

### New Capabilities

- `activity-visual-indicators`: Visual indicators, emoji labels, color badges, and styled cards for activity metrics on the event detail page

### Modified Capabilities

- `event-detail-page`: Activities display changes from plain table rows to visually enriched cards/rows with emoji labels and color-coded indicators

## Impact

- `Pages/EventDetail.cshtml` — activities section rewritten with visual indicators and enriched detail labels
- `wwwroot/css/site.css` — new CSS classes for activity cards, difficulty badges, metric indicators
- No backend, model, or service changes

## Non-goals

- No changes to the Activity data model or backend services
- No new data fetching or parsing logic
- No changes to pages other than EventDetail
