## MODIFIED Requirements

### Requirement: Display events in chronological list
The system SHALL display all planned mountain biking events on a dedicated List View page, ordered by start date ascending. Events SHALL be fetched client-side from the API.

#### Scenario: Events shown in date order
- **WHEN** the user navigates to the List View page
- **THEN** the app SHALL fetch events from `/api/events` and render them sorted by start date, earliest first

#### Scenario: Empty state
- **WHEN** the user navigates to the List View page and the API returns no events
- **THEN** the system SHALL display a message indicating there are no upcoming events

#### Scenario: Loading state
- **WHEN** the List View page is loading and the API request is in progress
- **THEN** the page SHALL display a loading indicator

### Requirement: List item shows key fields
Each event item in the list SHALL display the event date, title, and notes. The event title SHALL be a link to the Event Detail page.

#### Scenario: Event details visible
- **WHEN** the List View page is rendered with at least one event
- **THEN** each item SHALL show the event's date, title, and notes (notes may be empty)

#### Scenario: Event title links to detail page
- **WHEN** the List View page is rendered with at least one event
- **THEN** each event's title SHALL be a hyperlink that navigates to the Event Detail page for that event
