### Requirement: Fetch event details from popup API

The system SHALL fetch additional event details from the FFBC popup API endpoint when an event ID is available.

#### Scenario: Successfully fetch event details

- **WHEN** `GetEventDetailsAsync` is called with a valid event ID
- **THEN** the system SHALL POST to `ajax_request_popup_calendar.php` with the event ID and return the parsed event details

#### Scenario: Handle API failure gracefully

- **WHEN** the popup API request fails (network error, timeout, or non-success status)
- **THEN** the service SHALL return null without throwing an exception

#### Scenario: Handle invalid or empty response

- **WHEN** the popup API returns an empty or unparseable response
- **THEN** the service SHALL return null

### Requirement: Cache event details for performance

The system SHALL cache fetched event details to reduce API calls and improve response times.

#### Scenario: Cache hit returns cached details

- **WHEN** `GetEventDetailsAsync` is called for an event ID that was previously fetched within the cache duration
- **THEN** the system SHALL return the cached details without making an API request

#### Scenario: Cache miss fetches from API

- **WHEN** `GetEventDetailsAsync` is called for an event ID not in cache
- **THEN** the system SHALL fetch from the popup API and cache the result

#### Scenario: Cache expiration

- **WHEN** cached event details are older than 2 hours
- **THEN** the system SHALL fetch fresh details from the popup API

### Requirement: Extract event ID from calendar data

The system SHALL extract the event ID from the calendar HTML when parsing events.

#### Scenario: Event row has ID attribute

- **WHEN** parsing a calendar row that contains a data-id attribute
- **THEN** the Event SHALL have its EventId property set to that value

#### Scenario: Event row missing ID attribute

- **WHEN** parsing a calendar row without a data-id attribute
- **THEN** the Event SHALL have a null EventId property
