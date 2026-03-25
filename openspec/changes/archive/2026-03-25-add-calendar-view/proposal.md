## Why

The app has a list view for upcoming rides but no way to visualize events in a monthly calendar layout. A calendar view lets riders see how events are distributed across weeks and months at a glance, making it easier to plan around busy periods.

## What Changes

- Add a **Calendar View** page that displays events in a month-grid layout
- Show the current month by default with previous/next month navigation
- Each day cell shows any events scheduled for that date
- Add a "Calendar" link in the shared navigation bar

## Capabilities

### New Capabilities

- `calendar-view`: Display planned mountain biking events in a monthly calendar grid, with navigation between months

### Modified Capabilities

<!-- No existing specs are being changed -->

## Non-goals

- Clicking a day to create or edit an event (read-only for now)
- Week or day view modes
- Drag-and-drop rescheduling
- Integration with external calendars (e.g., Google, iCal)

## Impact

- New Razor Page (`Calendar.cshtml` / `Calendar.cshtml.cs`) added to the web project
- Reuses the existing `IEventStore` and `Event` model — no data layer changes
- Navigation menu updated with a "Calendar" link
- Page model must compute month grid layout from in-memory events
