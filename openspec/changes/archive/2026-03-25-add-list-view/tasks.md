## 1. Razor Page Scaffold

- [x] 1.1 Create `ListEvents.cshtml` Razor Page file with basic page structure and `@page` directive
- [x] 1.2 Create `ListEvents.cshtml.cs` PageModel class with `IEnumerable<Event>` property for the event list

## 2. Page Model Logic

- [x] 2.1 Inject the in-memory event store (or service) into `ListEvents.cshtml.cs`
- [x] 2.2 In `OnGet()`, retrieve all events and sort by start date ascending, assigning to the public property

## 3. Page View

- [x] 3.1 Render the sorted events as a `<ul>` or `<table>` showing date, title, and notes for each item
- [x] 3.2 Add an empty-state message (e.g., "No upcoming events") rendered when the event list is empty

## 4. Navigation

- [x] 4.1 Add a "List View" `<a>` link to `_Layout.cshtml` navigation bar alongside the existing Calendar link

## 5. Tests

- [x] 5.1 Write a unit test for `ListEvents.cshtml.cs.OnGet()` that asserts events are returned in ascending date order
- [x] 5.2 Write a unit test for `ListEvents.cshtml.cs.OnGet()` that asserts an empty list is returned when no events exist
