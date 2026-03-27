## Context

The event detail page currently displays activities in a plain `<table>` with text-only columns for Type, Distance, D+, Difficulty, Time, Ravito, and Price. All Activity model fields are strings. This change adds visual richness — emojis, color-coded badges, and styled layout — purely in the Razor template and CSS, without modifying the data model.

## Goals / Non-Goals

**Goals:**
- Add emoji labels to activity metric columns for instant visual scanning
- Color-code difficulty values (green → easy, yellow → moderate, orange → hard, red → expert)
- Style distance and elevation values with metric emphasis
- Style price with free/paid visual distinction
- Style ravito count with refreshment emoji
- Add emoji prefixes to detail section labels (Date, Town, Club, etc.)
- Keep all rendering logic in Razor (no new JS)

**Non-Goals:**
- Parsing numeric values from Activity string fields for computation (they stay as display strings)
- Changes to the Activity model or backend services
- Changes to any page other than EventDetail.cshtml

## Decisions

### Decision 1: Emoji + CSS badges over icon libraries

**Choice:** Use native Unicode emoji and CSS-styled badges for visual indicators rather than adding an icon library (FontAwesome, Bootstrap Icons).

**Rationale:** Zero dependency addition, native emoji render well across all modern browsers, and the existing design system already uses emoji (🚵, 📍, ℹ️, 🚴). CSS badges for difficulty levels give color coding without JS.

**Alternatives considered:**
- FontAwesome icons — adds ~100KB dependency, overkill for this scope
- SVG inline icons — more markup complexity, harder to maintain

### Decision 2: Activity cards layout instead of table rows

**Choice:** Replace the `<table>` with a card-per-activity layout using flexbox/grid, where each metric is a labeled pill/badge.

**Rationale:** A card layout gives more room for emoji + styled badges and is more readable on mobile than a 7-column table. Each metric gets its own visually distinct block.

**Alternatives considered:**
- Keep table with styled cells — too cramped for badges + emojis on narrow screens
- Accordion per activity — overkill, hides info behind clicks

### Decision 3: Difficulty color mapping via CSS classes

**Choice:** Map difficulty text to CSS class names using a simple Razor helper. Use semantic color tiers: `.difficulty-easy` (green), `.difficulty-moderate` (yellow), `.difficulty-hard` (orange), `.difficulty-expert` (red), `.difficulty-unknown` (gray).

**Rationale:** Difficulty values are freeform strings, so exact parsing is unreliable. A simple keyword/numeric check in Razor that falls back to "unknown" (gray) keeps logic minimal and safe.

## Risks / Trade-offs

- **Freeform string fields** → Difficulty, distance, elevation are unparsed strings. Color mapping uses best-effort heuristics; unrecognized values default to gray/neutral.
- **Emoji rendering varies** → Mitigation: Use widely-supported emoji (available since Unicode 6+), tested on Windows/macOS/iOS/Android.
- **Card layout takes more vertical space** → Trade-off accepted: better readability and visual appeal outweigh compactness of a table.
