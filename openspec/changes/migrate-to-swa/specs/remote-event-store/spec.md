## MODIFIED Requirements

### Requirement: Fetch events from FFBC website
The system SHALL retrieve mountain biking events by sending a POST request to the FFBC calendar API endpoint with the configured province, challenge, year, and date parameters. This logic SHALL run in an Azure Function rather than in the web application process.

#### Scenario: Successful fetch returns parsed events
- **WHEN** the event store is asked for all events and the cache is empty
- **THEN** the system SHALL POST to the configured URL with the required form-data payload and return the events parsed from the HTML response

#### Scenario: Request uses correct form-data fields
- **WHEN** the POST request is sent
- **THEN** it SHALL include `ajax=1`, `baseUrl`, `year_selected`, `challenge`, `province`, and `date` fields in `application/x-www-form-urlencoded` format

### Requirement: Cache fetched events
The system SHALL cache the parsed event list in memory for a configurable duration to avoid redundant remote requests. In the Azure Functions context, cache lifetime is bounded by the function instance lifetime.

#### Scenario: Cached result returned on subsequent calls
- **WHEN** the event store is called and a valid cached result exists within the same function instance
- **THEN** the system SHALL return the cached list without making an outbound HTTP request

#### Scenario: Cache expires after configured TTL
- **WHEN** the configured cache duration has elapsed since the last successful fetch
- **THEN** the system SHALL make a new HTTP request on the next call

### Requirement: Graceful degradation on fetch failure
The system SHALL return an empty event list when the remote fetch fails, rather than propagating an exception to callers.

#### Scenario: HTTP error returns empty list
- **WHEN** the HTTP request fails (network error or non-success status code)
- **THEN** the system SHALL return an empty list and SHALL NOT cache the failure

#### Scenario: Unparseable response returns empty list
- **WHEN** the HTTP response body cannot be parsed into events
- **THEN** the system SHALL return an empty list and SHALL NOT cache the failure

### Requirement: Request parameters are configurable
The system SHALL read the POST URL, province, challenge, year, and cache TTL from application configuration (environment variables in Azure Functions) rather than hard-coding them.

#### Scenario: Configuration drives request parameters
- **WHEN** the `FfbcEventStore` configuration section specifies a province and challenge value
- **THEN** the outbound POST request SHALL use those values in its form-data payload
