## Context

The FFBC Calendar is a server-side Razor Pages app (.NET 10) using Bootstrap 5 as its CSS foundation. The current UI uses raw Bootstrap defaults with hard-coded hex color values (`#1b6ec2`, `#1861ac`, `#0077cc`), no CSS custom properties, minimal typographic scale, and no cohesive design system. The result is a functional but visually flat application that feels generic and unpolished.

The goal is to layer a modern design system on top of Bootstrap 5 — introducing CSS custom properties, a refined color palette, improved typography, and per-component visual polish — without changing any backend logic or replacing Bootstrap.

## Goals / Non-Goals

**Goals:**
- Define a CSS custom property (design token) layer in `site.css`
- Introduce a modern, sports-oriented color palette with proper contrast ratios
- Improve typography: font stack, sizing scale, weight, line-height
- Redesign the navbar for visual impact and clarity
- Polish the event list (better table/card visual hierarchy)
- Polish the calendar grid (cleaner cells, readable event chips)
- Polish the event detail page (info cards, grouped sections)
- Polish the map page layout and side panel
- Add subtle transitions and hover/focus states throughout
- All changes via CSS only (no new JS, no backend changes)

**Non-Goals:**
- Dark mode support
- Migration away from Bootstrap 5
- New pages or navigation items
- Any backend, service, or data model changes
- Accessibility audit (separate initiative)
- Tailwind CSS or other CSS framework migration

## Decisions

### Decision 1: CSS Custom Properties over SASS/LESS variables

**Choice:** Introduce CSS custom properties (`:root` variables) in `site.css` rather than introducing a preprocessor build step.

**Rationale:** The project has no current build pipeline for CSS. Adding SASS would require new tooling. CSS variables are supported in all modern browsers, allow runtime theming in future, and achieve the same goal with zero tooling change.

**Alternatives considered:**
- Bootstrap SASS customization — requires build pipeline, overkill for this scope
- Inline overrides per-component — hard to maintain, no single source of truth

---

### Decision 2: Keep Bootstrap 5, override selectively

**Choice:** Retain Bootstrap 5 and use CSS specificity / custom properties to override its defaults.

**Rationale:** Bootstrap provides the responsive grid, utilities, and base normalization. Replacing it would be a much larger change and risk regressions. A thin override layer achieves the visual goals without that risk.

**Alternatives considered:**
- Full replacement with Tailwind CSS — too broad in scope, breaks existing markup
- Manual CSS-only from scratch — too much effort, high regression risk

---

### Decision 3: Sports-inspired color palette anchored in deep blue/teal

**Choice:** Use a modern deep navy/teal primary palette with warm accent for CTAs, replacing the legacy `#1b6ec2` flat blue.

**Rationale:** Mountain biking is an outdoor sport — a deep navy (#0f172a) with a vibrant teal/blue accent (#0ea5e9) and an energetic orange CTA (#f97316) conveys energy, trust, and modernity. High contrast ensures readability.

---

### Decision 4: One unified `site.css` for tokens + overrides

**Choice:** All design tokens and Bootstrap overrides live in `wwwroot/css/site.css`. The scoped `_Layout.cshtml.css` is emptied/removed.

**Rationale:** Eliminates split between two files, single place to maintain the design system.

## Risks / Trade-offs

- **Bootstrap specificity conflicts** → Mitigation: Use `:root` variables to feed Bootstrap's own custom properties (Bootstrap 5 already uses CSS vars internally); use specific class selectors only where necessary.
- **Rendering differences across pages** → Mitigation: Test each page after CSS changes. Each page is independent.
- **Calendar grid complexity** → Mitigation: The calendar uses a custom table structure; target it with specific `.calendar-*` classes rather than global overrides.
- **Leaflet map controls styled separately** → Mitigation: Leaflet uses its own CSS; only the surrounding page layout is in scope.

## Migration Plan

1. Add CSS custom properties to `:root` in `site.css`
2. Override Bootstrap's own CSS vars (e.g., `--bs-primary`) to propagate palette throughout
3. Rewrite navbar styles
4. Rewrite page-level styles per page (Index → ListEvents → Calendar → EventDetail → Map)
5. Clear `_Layout.cshtml.css` of redundant rules
6. Manual visual review of all pages in browser
7. No rollback complexity — pure CSS change; reverting is a `git revert`
