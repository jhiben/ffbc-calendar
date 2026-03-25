## Context

The app is a Razor Pages (.NET) mountain biking calendar using in-memory storage. Currently, events are displayed only in a calendar grid view. Users need a fast, linear way to review upcoming rides. The list view is an additive, read-only page with no changes to the existing data model or storage layer.

## Goals / Non-Goals

**Goals:**
- Add a new Razor Page that displays all in-memory events in ascending date order
- Provide navigation between the existing calendar view and the new list view
- Reuse the existing in-memory event model without modification

**Non-Goals:**
- Changing or extending the event data model
- Adding filtering, search, or pagination to the list
- Editing events from the list view
- Persisting data (no database)

## Decisions

### Single new Razor Page (`ListEvents.cshtml`)

**Decision**: Add one `ListEvents.cshtml` / `ListEvents.cshtml.cs` Razor Page rather than a partial view or a view component.

**Rationale**: The list view is a standalone destination reachable by URL, not an embedded widget. Razor Pages align with the existing project pattern. A partial view would require host-page scaffolding; a view component adds unnecessary complexity for a simple read-only list.

**Alternatives considered**:
- _View component_: Overhead not justified for a full-page feature
- _Modify existing calendar page_: Would couple two distinct UI modes, increasing complexity

### Sort by ascending start date in the page model

**Decision**: Ordering is applied in the `PageModel.OnGet()` by sorting the in-memory collection, not in a separate service.

**Rationale**: The data store is in-memory and small; a full service layer for read-only sorting would be premature. This keeps the change self-contained.

### Navigation via shared layout link

**Decision**: Add a "List View" link in the shared `_Layout.cshtml` navigation bar alongside the existing "Calendar" link.

**Rationale**: Centralized navigation ensures the option is visible from all pages, consistent with existing layout conventions.

## Risks / Trade-offs

| Risk | Mitigation |
|---|---|
| In-memory list grows large over time → slow page render | Acceptable for current scope; pagination can be added later |
| Navigation bar change touches shared layout → broad visual impact | Change is additive (one `<a>` tag); low regression risk |

## Migration Plan

No data migration required. The change is purely additive:
1. Add `ListEvents.cshtml` + `ListEvents.cshtml.cs`
2. Update `_Layout.cshtml` nav bar
3. Verify existing calendar page is unaffected

Rollback: revert the two new files and the nav bar change.
