## Context

The app is a C# .NET Razor Pages mountain biking planning tool using in-memory storage. A list view was recently added showing events chronologically. The next step is a calendar view — a month grid where riders can see how rides are spread across a month and navigate between months. No data model changes are required; the existing `IEventStore` provides all needed data.

## Goals / Non-Goals

**Goals:**
- Add a `Calendar.cshtml` Razor Page rendering a month grid for a given year/month
- Default to the current month on first load
- Support previous/next month navigation via query string parameters
- Show event titles on the day cells where they fall
- Reuse `IEventStore` with no modifications

**Non-Goals:**
- Creating or editing events from the calendar
- Week or day granularity views
- Highlighting "today" beyond what is naturally supported by the date comparison
- External calendar format support (iCal, Google)

## Decisions

### Month grid computed in the PageModel

**Decision**: Build the month grid (a 2D array of day cells, each holding a date and its events) in `Calendar.cshtml.cs.OnGet()`, not in a helper service or view component.

**Rationale**: The computation is straightforward (DateTime arithmetic) and specific to this page. Extracting it to a service would add abstraction without benefit for a single consumer.

**Alternatives considered**:
- _View component_: Unnecessary indirection for a full-page feature
- _Shared calendar service_: Premature generalization; no other consumer today

### Month navigation via query string (`?year=YYYY&month=MM`)

**Decision**: Pass `year` and `month` as query string parameters to `OnGet(int? year, int? month)`. Default to `DateTime.Today` if omitted.

**Rationale**: Bookmarkable, shareable URLs with no server-side session state. Consistent with Razor Pages conventions for read-only parametric pages.

**Alternatives considered**:
- _Route parameters_ (`/Calendar/2026/04`): Slightly more REST-like but requires route template changes and is harder to generate with `asp-page` tag helpers
- _POST for navigation_: Inappropriate for a read-only view

### Navigation link in shared layout

**Decision**: Add a "Calendar" `<a>` link in `_Layout.cshtml` alongside the existing "List View" link.

**Rationale**: Consistent with the pattern established by the list view change.

## Risks / Trade-offs

| Risk | Mitigation |
|---|---|
| Month grid logic is slightly complex (first day offset, trailing days) | Use `DateTime.DaysInMonth` and `(int)firstDay.DayOfWeek` — standard .NET, well-tested pattern |
| Events on the last day of a month not shown if grid cell count is off-by-one | Cover with a unit test that asserts events appear on the correct day cell |

## Migration Plan

Purely additive change — no data migration needed.

1. Add `Calendar.cshtml` + `Calendar.cshtml.cs`
2. Update `_Layout.cshtml` nav bar
3. Verify list view and other pages unaffected

Rollback: revert the two new page files and the nav bar edit.
