## 1. Design Tokens Foundation

- [x] 1.1 Define CSS custom properties on `:root` in `site.css`: color palette (`--color-primary`, `--color-primary-dark`, `--color-accent`, `--color-surface`, `--color-surface-raised`, `--color-text`, `--color-text-muted`, `--color-border`)
- [x] 1.2 Define typography tokens in `:root`: `--font-sans`, `--font-size-sm`, `--font-size-base`, `--font-size-lg`, `--font-size-xl`, `--font-size-2xl`, `--font-weight-normal`, `--font-weight-medium`, `--font-weight-bold`, `--line-height-base`
- [x] 1.3 Define utility tokens in `:root`: `--radius-sm`, `--radius-md`, `--radius-lg`, `--shadow-sm`, `--shadow-md`, `--transition-base`
- [x] 1.4 Override Bootstrap 5 CSS custom properties (`--bs-primary`, `--bs-body-font-family`, `--bs-body-color`, `--bs-border-color`, etc.) to feed from the design tokens

## 2. Global Base & Typography

- [x] 2.1 Apply `--font-sans` to `body`, set `--font-size-base` and `--line-height-base`
- [x] 2.2 Style h1–h4 headings with appropriate size, weight, and line-height from the typography scale
- [x] 2.3 Style global links with `--color-accent`, remove underline by default, underline on hover
- [x] 2.4 Add global focus-visible ring style using `--color-accent` (keyboard navigation, WCAG 2.4.7)

## 3. Navigation Bar

- [x] 3.1 Restyle `.navbar` in `site.css`: deep navy background (`--color-primary`), white brand + links, proper padding
- [x] 3.2 Add active link indicator (bottom border or background highlight using `--color-accent`)
- [x] 3.3 Add hover transition on nav links using `--transition-base`
- [x] 3.4 Clean up `_Layout.cshtml.css`: remove duplicate navbar rules now covered by site.css tokens

## 4. Shared Components

- [x] 4.1 Style `.btn-primary` and `.btn-outline-primary` using tokens; add hover transition
- [x] 4.2 Style `.badge` elements using tokens (used for event challenge tags)
- [x] 4.3 Style `.card` components (border, radius, shadow) using `--shadow-sm`, `--radius-md`
- [x] 4.4 Style `.table` globally: header background from `--color-surface-raised`, hover row highlight, border from `--color-border`

## 5. Home Page (Index)

- [x] 5.1 Add a hero/intro section with a styled heading, subtitle, and clear CTA links
- [x] 5.2 Style the quick-nav links/cards on the home page for visual appeal

## 6. Event List Page

- [x] 6.1 Style the events table with improved column widths, header weight, and row spacing
- [x] 6.2 Add row hover state using `--color-surface-raised` and `--transition-base`
- [x] 6.3 Make the event date column visually prominent (bold or accent color)
- [x] 6.4 Style filter/search controls if present (inputs, selects) with token-based border and radius

## 7. Calendar Page

- [x] 7.1 Style the calendar grid cells: consistent padding, border from `--color-border`, radius
- [x] 7.2 Highlight today's cell with `--color-accent` tint background
- [x] 7.3 Style event chips inside day cells: compact height, contrast ratio ≥4.5:1, overflow ellipsis
- [x] 7.4 Style the month navigation (prev/next) using token-based buttons
- [x] 7.5 Ensure calendar is readable at mobile breakpoints (check Bootstrap grid usage)

## 8. Event Detail Page

- [x] 8.1 Wrap the general info section (date, location, club) in a card with `--shadow-sm`
- [x] 8.2 Wrap the activities table section in a card; style the table inside using global table tokens
- [x] 8.3 Style the registration CTA button prominently with accent color
- [x] 8.4 Add visual section headings (icon or colored left-border) to distinguish sections

## 9. Map Page

- [x] 9.1 Set map container to a proper height (e.g., 500px), add `--radius-md` and `--color-border` border
- [x] 9.2 Style the unmapped events table below the map using global table tokens
- [x] 9.3 Style the geolocation / filter panel or controls on the map page

## 10. Final Polish & Cleanup

- [x] 10.1 Remove all hard-coded hex colors (`#1b6ec2`, `#1861ac`, `#0077cc`, `#e5e5e5`) from `site.css` and `_Layout.cshtml.css`
- [x] 10.2 Remove redundant rules from `_Layout.cshtml.css` that are now covered by site.css
- [x] 10.3 Visual review: open each page in browser and verify consistent look/feel
- [x] 10.4 Verify no Bootstrap component is visually broken (navbar collapse, badges, tables, buttons)
