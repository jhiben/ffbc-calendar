## 1. Razor Page Scaffold

- [x] 1.1 Create `Calendar.cshtml` Razor Page file with `@page` directive and query string parameters for year and month
- [x] 1.2 Create `Calendar.cshtml.cs` PageModel class with properties for the displayed month and the grid data structure

## 2. Page Model Logic

- [x] 2.1 Inject `IEventStore` into `Calendar.cshtml.cs`
- [x] 2.2 In `OnGet(int? year, int? month)`, default to `DateTime.Today` when parameters are absent and resolve the target month
- [x] 2.3 Compute the month grid: determine the starting day-of-week offset and build a list of week rows, each containing 7 day cells with their date and any matching events
- [x] 2.4 Expose previous-month and next-month `(year, month)` values as properties for nav link generation

## 3. Page View

- [x] 3.1 Render the month heading (e.g., "April 2026") with Previous / Next navigation links using `asp-page` and `asp-route-*` tag helpers
- [x] 3.2 Render the calendar grid as an HTML table with day-of-week headers (Sun–Sat)
- [x] 3.3 Render event titles inside the appropriate day cells; show cells outside the current month as empty/greyed

## 4. Navigation

- [x] 4.1 Add a "Calendar" `<a>` link to `_Layout.cshtml` navigation bar alongside the existing "List View" link

## 5. Tests

- [x] 5.1 Write a unit test for `Calendar.cshtml.cs.OnGet()` verifying that events appear in the correct week row and day cell for a known month
- [x] 5.2 Write a unit test for `Calendar.cshtml.cs.OnGet()` verifying that with no parameters the model defaults to the current year and month
- [x] 5.3 Write a unit test for `Calendar.cshtml.cs.OnGet()` verifying that previous-month and next-month navigation values are computed correctly (including year boundary wrap)
