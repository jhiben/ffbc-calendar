## Why

The FFBC Calendar app is functional but visually dated — it uses raw Bootstrap 5 defaults with hard-coded colors, no design tokens, and minimal visual polish. Users deserve a modern, crisp, and usable interface that reflects the quality of the content it delivers.

## What Changes

- Replace hard-coded hex colors with CSS custom properties (design tokens)
- Introduce a modern color palette, typography scale, and spacing system
- Redesign the navigation bar for a cleaner, more modern appearance
- Modernize the event list with card-based or enhanced table layout
- Modernize the calendar grid with better visual hierarchy and event indicators
- Modernize the event detail page with structured info cards and visual grouping
- Improve map page layout and info panel styling
- Polish interactive states: hover, focus, active, transitions
- Improve overall readability: contrast, font sizing, line height

## Capabilities

### New Capabilities

- `design-system`: CSS design tokens, color palette, typography scale, spacing, and component-level styles that unify the visual language across all pages

### Modified Capabilities

*(No spec-level behavior changes — all existing functional specs remain valid)*

## Impact

- `wwwroot/css/site.css` — rewritten with CSS variables and modern styles
- `Pages/Shared/_Layout.cshtml` — navbar and layout structure updates
- `Pages/Shared/_Layout.cshtml.css` — replaced by site.css tokens
- `Pages/Index.cshtml` — visual polish
- `Pages/ListEvents.cshtml` — modernized table/card layout
- `Pages/Calendar.cshtml` — improved calendar grid styling
- `Pages/EventDetail.cshtml` — structured info cards
- `Pages/Map.cshtml` — improved layout and panel styling
- No backend, API, or data model changes

## Non-goals

- No changes to data models, services, or backend logic
- No migration away from Bootstrap 5 (we enhance, not replace)
- No dark mode (out of scope for this iteration)
- No new pages or navigation items
- No JavaScript framework migration
