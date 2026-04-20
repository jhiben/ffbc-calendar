## MODIFIED Requirements

### Requirement: Display events in monthly calendar grid
The system SHALL display planned mountain biking events in a monthly calendar grid showing all days of the selected month, with weekly columns ordered Monday through Sunday. Calendar grid logic SHALL be computed client-side in the Svelte component rather than server-side. Each event badge SHALL link to the Event Detail page.

#### Scenario: Default to current month
- **WHEN** the user navigates to the Calendar page with no query string parameters
- **THEN** the system SHALL render the calendar for the current year and month

#### Scenario: Display events on correct day cells
- **WHEN** the Calendar page is rendered for a given month
- **THEN** the app SHALL fetch events from `/api/events` and display events on the correct day cells

#### Scenario: Empty day cells shown
- **WHEN** the Calendar page is rendered for a given month
- **THEN** days with no events SHALL still appear as empty cells in the grid

#### Scenario: Calendar week starts on Monday
- **WHEN** the Calendar page is rendered for any month
- **THEN** the calendar grid SHALL place Monday in the first weekly column and Sunday in the last weekly column

#### Scenario: Event badges link to detail page
- **WHEN** the Calendar page is rendered with events
- **THEN** each event badge SHALL be a hyperlink that navigates to the Event Detail page for that event

### Requirement: Navigate between months
The system SHALL allow users to navigate to the previous and next month from the Calendar page using client-side navigation.

#### Scenario: Navigate to previous month
- **WHEN** the user activates the "Previous" navigation control
- **THEN** the system SHALL render the calendar for the month immediately before the currently displayed month without a full page reload

#### Scenario: Navigate to next month
- **WHEN** the user activates the "Next" navigation control
- **THEN** the system SHALL render the calendar for the month immediately after the currently displayed month without a full page reload

#### Scenario: Navigate to a specific month via URL
- **WHEN** the user navigates to the Calendar page with `?year=YYYY&month=MM` query parameters
- **THEN** the system SHALL render the calendar for the specified year and month
