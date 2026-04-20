## MODIFIED Requirements

### Requirement: Display full event details on a dedicated page
The system SHALL provide a dedicated Event Detail page that displays all available information for a single event, identified by date and title. Event data SHALL be fetched client-side from the API.

#### Scenario: Detail page shows all event fields
- **WHEN** the user navigates to the Event Detail page with a valid date and title
- **THEN** the app SHALL fetch the event from `/api/events`, find the matching event, and display the event's title, date, town, postal code, country, province, challenge type, and notes

#### Scenario: Detail page renders a user-friendly layout
- **WHEN** the Event Detail page is rendered for an event
- **THEN** the system SHALL present the event information in a structured, readable layout with clear labels for each field

#### Scenario: Missing optional fields are handled gracefully
- **WHEN** an event has null or empty values for optional fields (town, postal code, province, challenge)
- **THEN** the detail page SHALL omit those fields from the display rather than showing blank labels

### Requirement: Event not found handling
The system SHALL display a meaningful message when the requested event cannot be found.

#### Scenario: Event not found by date and title
- **WHEN** the user navigates to the Event Detail page with a date and title that do not match any event from the API
- **THEN** the system SHALL display a "not found" message and a link back to the event list

#### Scenario: Missing parameters
- **WHEN** the user navigates to the Event Detail page without providing both date and title parameters
- **THEN** the system SHALL redirect to the event list page

### Requirement: Display enriched event details
The Event Detail page SHALL display additional information fetched from the event details API endpoint when available. Activities SHALL be rendered as visually enriched cards with emoji-labeled metrics and color-coded badges.

#### Scenario: Enriched details fetched from API
- **WHEN** the Event Detail page loads for an event with a valid EventId
- **THEN** the app SHALL fetch enriched details from `/api/events/{eventId}/details` and display them alongside the basic event information

#### Scenario: Page works without enriched details
- **WHEN** the Event Detail page loads for an event without an EventId or when the details API returns 404
- **THEN** the page SHALL display the basic event information without enriched details

#### Scenario: Enriched details section hidden when empty
- **WHEN** enriched details are not available for an event
- **THEN** the page SHALL NOT display an empty enriched details section

#### Scenario: Activities rendered as visual cards
- **WHEN** enriched details include one or more activities
- **THEN** the page SHALL render each activity as a styled card with emoji-labeled metric badges

### Requirement: Loading state while fetching details
The Event Detail page SHALL show a loading indicator while fetching data from the API.

#### Scenario: Loading state displayed during fetch
- **WHEN** the Event Detail page is loading and API requests are in progress
- **THEN** the page SHALL display a loading spinner or skeleton placeholder

#### Scenario: Loading state cleared after fetch
- **WHEN** API requests complete (success or failure)
- **THEN** the loading indicator SHALL be replaced with the event content or error message
