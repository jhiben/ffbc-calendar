## Why

The calendar currently starts weeks on Sunday, which is inconsistent with user expectations in many European locales, including Luxembourg. Making Monday the first day improves readability and planning accuracy for the target audience.

## What Changes

- Update calendar rendering behavior so weekly columns start on Monday and end on Sunday.
- Ensure day header labels and date-to-column mapping align with Monday-first ordering.
- Preserve existing event grouping and display behavior while changing only week start semantics.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `calendar-view`: Week layout requirements change from Sunday-first to Monday-first ordering.

## Impact

- Affected UI: calendar page rendering and day header order in `src/WebApp/Pages/Calendar.cshtml` and related page model logic.
- Affected tests: calendar-focused unit tests in `tests/WebApp.Tests/CalendarTests.cs`.
- No API contract changes and no new external dependencies.

## Non-goals

- No redesign of calendar visuals or styling.
- No change to list view behavior.
- No locale auto-detection or per-user week-start preferences in this change.
