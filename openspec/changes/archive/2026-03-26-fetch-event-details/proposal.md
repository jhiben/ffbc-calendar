## Why

The Event Detail page currently displays only the basic information parsed from the calendar listing (title, date, location). The FFBC website has a separate popup API that provides richer event details. By fetching this additional information, users can see more comprehensive event data without leaving our application.

## What Changes

- Create a new service to fetch additional event details from the FFBC popup API endpoint
- Cache fetched details for 2 hours to reduce API calls and improve performance
- Extend the Event Detail page to display the additional fetched information
- Add new properties to hold the enriched event data

## Capabilities

### New Capabilities

- `event-details-enrichment`: Fetch and cache additional event details from the FFBC popup API using the event ID

### Modified Capabilities

- `event-detail-page`: Display additional enriched fields fetched from the popup API alongside existing event information

## Impact

- `src/WebApp/Services/`: New service for fetching event details from popup endpoint
- `src/WebApp/Pages/EventDetail.cshtml`: Updated to display additional fields
- `src/WebApp/Pages/EventDetail.cshtml.cs`: Call enrichment service when loading event
- HTTP client configuration: Reuse existing "ffbc" named client

## Non-goals

- Modifying the calendar event fetching logic (separate endpoint)
- Persisting enriched data to a database
- Pre-fetching details for all events (only fetch on-demand when viewing detail page)
