### Requirement: Display events in chronological list
The system SHALL display all planned mountain biking events on a dedicated List View page, ordered by start date ascending.

#### Scenario: Events shown in date order
- **WHEN** the user navigates to the List View page
- **THEN** the system SHALL render all in-memory events sorted by start date, earliest first

#### Scenario: Empty state
- **WHEN** the user navigates to the List View page and no events exist
- **THEN** the system SHALL display a message indicating there are no upcoming events

### Requirement: List item shows key fields
Each event item in the list SHALL display the event date, title, and notes.

#### Scenario: Event details visible
- **WHEN** the List View page is rendered with at least one event
- **THEN** each item SHALL show the event's date, title, and notes (notes may be empty)

### Requirement: Navigation to list view
The application navigation SHALL include a link to the List View page accessible from any page.

#### Scenario: List View link present in nav bar
- **WHEN** the user is on any page in the application
- **THEN** the shared navigation bar SHALL contain a "List View" link that navigates to the List View page

#### Scenario: Calendar view remains accessible
- **WHEN** the user is on the List View page
- **THEN** the existing Calendar link SHALL remain in the navigation bar and navigate back to the calendar
