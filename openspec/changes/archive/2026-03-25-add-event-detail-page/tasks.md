## 1. Enrich Event Model & Parser

- [x] 1.1 Add `Challenge` (string?) and `Province` (string?) properties to `Event.cs`
- [x] 1.2 Update `FfbcWebEventStore.TryParseEvents` to populate `Challenge` from the challenge column and `Province` from the province prefix in place text
- [x] 1.3 Update `FfbcWebEventStore.ParsePlace` to output the province as a separate value
- [x] 1.4 Update existing parser tests to verify the new `Challenge` and `Province` fields are populated correctly
- [x] 1.5 Add parser tests for events with and without province prefix and challenge text

## 2. Event Lookup

- [x] 2.1 Add `Event? GetByDateAndTitle(DateTime date, string title)` to `IEventStore`
- [x] 2.2 Implement `GetByDateAndTitle` in `InMemoryEventStore` with case-insensitive title matching
- [x] 2.3 Implement `GetByDateAndTitle` in `FfbcWebEventStore` by filtering `GetAll()`
- [x] 2.4 Add unit tests for `GetByDateAndTitle` — match found, no match, case-insensitive match

## 3. Event Detail Page

- [x] 3.1 Create `EventDetail.cshtml.cs` page model with `date` and `title` query parameters, calling `GetByDateAndTitle`
- [x] 3.2 Create `EventDetail.cshtml` Razor view displaying all event fields in a structured layout
- [x] 3.3 Handle not-found case: show message and link back to list view
- [x] 3.4 Handle missing parameters: redirect to list view
- [x] 3.5 Add unit tests for Event Detail page model (event found, not found, missing params)

## 4. Link from Existing Views

- [x] 4.1 Update `ListEvents.cshtml` to make event titles clickable links to the detail page
- [x] 4.2 Update `Calendar.cshtml` to wrap event badges in links to the detail page
- [x] 4.3 Update `Map.cshtml` marker popup to include a link to the detail page
- [x] 4.4 Update `Map.cshtml` unmapped events table to link event titles to the detail page
- [x] 4.5 Update or add tests verifying that links to the detail page are rendered on list, calendar, and map views
