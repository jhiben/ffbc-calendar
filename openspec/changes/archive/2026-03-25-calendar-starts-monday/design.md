## Context

The application already renders a monthly calendar view for events. Current week layout starts on Sunday due to default date-index logic and/or header ordering. For the FFBC audience (Luxembourg/Europe), Monday-first is the expected weekly order and improves planning consistency.

## Goals / Non-Goals

**Goals:**
- Make the calendar grid start each week on Monday.
- Keep event-to-date placement correct after the week-start change.
- Keep month navigation and existing page routes unchanged.

**Non-Goals:**
- No visual redesign of calendar styling.
- No localization framework changes.
- No per-user setting for configurable week start.

## Decisions

- Use Monday-first index mapping in calendar grid generation.
  - Rationale: Root-cause fix in date-to-cell computation avoids view-only hacks and ensures correctness for all months.
  - Alternative considered: Reorder only weekday headers. Rejected because events and blank leading cells could become misaligned.
- Keep existing data source and page model surface intact.
  - Rationale: Requirement change is display semantics only; no store/API contract changes are required.
- Update tests to assert Monday-first offsets and header/day alignment.
  - Rationale: Prevent regressions and document expected behavior clearly.

## Risks / Trade-offs

- [Risk] Off-by-one errors in first-week offset when month starts on Sunday. → Mitigation: Explicit conversion formula from .NET `DayOfWeek` to Monday-first index and targeted tests.
- [Risk] Existing tests may encode Sunday-first assumptions. → Mitigation: Update/replace assertions to validate Monday-first behavior.
- [Trade-off] Hardcoded Monday-first behavior for now. → Mitigation: Keep logic isolated so future configurable week start can be added safely.

## Migration Plan

- Implement Monday-first mapping in calendar computation.
- Verify day headers and rendered cells align with expected dates.
- Run unit tests focused on calendar behavior.
- Rollback strategy: restore previous day-index mapping if unexpected rendering defects are found.

## Open Questions

- None for this change.
