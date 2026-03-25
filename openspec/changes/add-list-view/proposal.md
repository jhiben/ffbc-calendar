## Why

The app currently shows mountain biking events only as a calendar grid, making it difficult to quickly scan upcoming rides in chronological order. A list view gives riders a fast, linear summary of what's coming up without navigating the full calendar.

## What Changes

- Add a **List View** page that displays all planned rides and events in chronological order
- Add a toggle or navigation link to switch between the existing calendar view and the new list view
- Each list item shows key details: date, title, and relevant notes

## Capabilities

### New Capabilities

- `list-view`: Display planned mountain biking events as a sortable, chronological list with date, title, and notes

### Modified Capabilities

<!-- No existing specs are being changed -->

## Non-goals

- Filtering or searching the list (can be added in a future change)
- Editing events directly from the list view
- Pagination (in-memory storage means the list remains small)

## Impact

- New Razor Page (`ListEvents` or similar) added to the web project
- Shared event model used by both calendar and list views — no structural change to in-memory store
- Navigation menu updated with a "List View" link
