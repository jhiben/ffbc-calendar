## 1. Calendar Grid Week-Start Logic

- [x] 1.1 Update calendar day-offset computation in `Calendar.cshtml.cs` to use Monday-first indexing (Monday=0 ... Sunday=6)
- [x] 1.2 Ensure generated week rows/cells preserve correct date placement and leading/trailing empty cells after Monday-first conversion

## 2. Calendar View Alignment

- [x] 2.1 Update weekday header rendering in `Calendar.cshtml` to display Monday through Sunday order
- [x] 2.2 Verify event titles remain rendered on the same actual dates after header/order change

## 3. Test Coverage

- [x] 3.1 Update/add `CalendarTests.cs` cases to assert Monday-first first-column behavior for representative months (including months starting on Sunday)
- [x] 3.2 Add/adjust tests validating header order and date-to-cell alignment to prevent regressions

## 4. Validation

- [x] 4.1 Run `WebApp.Tests` and confirm all calendar-related tests pass
- [x] 4.2 Manually verify calendar page navigation between months still works with Monday-first layout
