## ADDED Requirements

### Requirement: CSS design tokens via custom properties
The system SHALL define a set of CSS custom properties on `:root` in `site.css` covering color palette, typography scale, spacing, border radius, shadow levels, and transition durations. All component styles SHALL reference these tokens rather than hard-coded values.

#### Scenario: Color tokens available globally
- **WHEN** any page is rendered
- **THEN** CSS custom properties `--color-primary`, `--color-primary-dark`, `--color-accent`, `--color-surface`, `--color-surface-raised`, `--color-text`, `--color-text-muted`, `--color-border` SHALL be available on `:root`

#### Scenario: Bootstrap color system overridden via tokens
- **WHEN** any Bootstrap component (btn-primary, navbar, badge, etc.) renders
- **THEN** it SHALL use the custom palette (deep navy/teal) instead of Bootstrap's default blue

---

### Requirement: Modern typography system
The system SHALL define a consistent typography scale using CSS custom properties for font family, size steps, weight, and line height.

#### Scenario: Font stack applied site-wide
- **WHEN** any page is rendered
- **THEN** body text SHALL use a modern sans-serif stack (system-ui / Inter / Segoe UI) defined via `--font-sans`

#### Scenario: Heading hierarchy is visually distinct
- **WHEN** a page contains h1–h4 headings
- **THEN** each heading level SHALL have a distinct font size, weight, and line height drawn from the typography scale

---

### Requirement: Modern navigation bar
The system SHALL render a sleek, full-width navigation bar with clear brand identity, readable nav links, a visible active-link indicator, and appropriate hover states.

#### Scenario: Navbar uses design tokens
- **WHEN** the navbar is rendered
- **THEN** it SHALL use `--color-primary` background, white text, and a bottom accent on the active link

#### Scenario: Navbar links show hover feedback
- **WHEN** a user hovers a nav link
- **THEN** the link SHALL transition to a lighter color with a subtle underline or highlight using `--transition-base`

---

### Requirement: Event list visual polish
The system SHALL present the event list in a visually polished layout with clear row hierarchy, readable columns, and hover feedback.

#### Scenario: Table rows have hover state
- **WHEN** a user hovers an event row in the list
- **THEN** the row SHALL highlight with `--color-surface-raised` background and a smooth transition

#### Scenario: Event date is visually prominent
- **WHEN** an event row is rendered
- **THEN** the date column SHALL use a distinct weight or color to aid scanning

---

### Requirement: Calendar grid visual polish
The system SHALL render the calendar month view with clearly delineated day cells, today highlight, event chips with proper contrast, and smooth visual hierarchy.

#### Scenario: Today's date is highlighted
- **WHEN** the current month's calendar is rendered
- **THEN** today's cell SHALL have a distinct background using `--color-accent` or a derived tint

#### Scenario: Event chips are readable
- **WHEN** an event appears on a calendar day
- **THEN** it SHALL render as a compact chip with adequate contrast ratio (≥4.5:1) between text and background

---

### Requirement: Event detail page visual polish
The system SHALL render the event detail page with structured info sections displayed as cards, clear section headings, and a visually distinct activities table.

#### Scenario: Detail sections use card layout
- **WHEN** an event detail page is rendered
- **THEN** each logical section (general info, location, activities) SHALL be wrapped in a card using `--color-surface-raised` and `--shadow-sm`

---

### Requirement: Map page layout polish
The system SHALL render the map page with a clean full-height map container, styled info panel, and polished "unmapped events" table below the map.

#### Scenario: Map container has proper height and border
- **WHEN** the map page is rendered
- **THEN** the map SHALL fill available vertical space with a defined height, rounded corners, and a subtle border using `--color-border`

---

### Requirement: Interactive state polish
All interactive elements (buttons, links, table rows, cards) SHALL have consistent hover, focus, and active states using CSS transitions.

#### Scenario: Buttons show transition on hover
- **WHEN** a user hovers any `.btn-primary` button
- **THEN** the button SHALL darken/lighten smoothly using `transition: var(--transition-base)`

#### Scenario: Focus rings are visible
- **WHEN** a user navigates with keyboard and focuses any interactive element
- **THEN** a visible focus ring using `--color-accent` SHALL appear (WCAG 2.4.7 compliance)
