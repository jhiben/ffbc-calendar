## 1. Event Model Updates

- [x] 1.1 Add EventId property (string?) to the Event model
- [x] 1.2 Create EventDetails model to hold enriched data from popup API

## 2. Event ID Extraction

- [x] 2.1 Update FfbcWebEventStore.TryParseEvents to extract data-id attribute from calendar rows
- [x] 2.2 Set EventId property on parsed Event objects
- [x] 2.3 Add unit tests for event ID extraction (with and without data-id attribute)

## 3. Event Details Service

- [x] 3.1 Create IEventDetailsService interface with GetEventDetailsAsync method
- [x] 3.2 Create FfbcEventDetailsService implementing the interface
- [x] 3.3 Implement POST request to popup API endpoint with event ID
- [x] 3.4 Parse popup API response into EventDetails model
- [x] 3.5 Add caching with 2-hour TTL using IMemoryCache
- [x] 3.6 Handle API failures gracefully (return null)
- [x] 3.7 Register service in Program.cs DI container

## 4. Event Detail Page Updates

- [x] 4.1 Inject IEventDetailsService into EventDetailModel
- [x] 4.2 Fetch enriched details when EventId is available
- [x] 4.3 Add EventDetails property to page model
- [x] 4.4 Update EventDetail.cshtml to display enriched details section
- [x] 4.5 Hide enriched details section when data is not available

## 5. Testing & Verification

- [x] 5.1 Add unit tests for FfbcEventDetailsService (cache hit, cache miss, API failure)
- [x] 5.2 Verify existing tests still pass
- [x] 5.3 Manual test: View event detail page with enriched data
- [x] 5.4 Manual test: Verify graceful degradation when details unavailable
